#include "ParquetWriter.h"
#include <arrow/io/api.h>
#include <parquet/arrow/reader.h>
#include <parquet/arrow/writer.h>
#include <iostream>
#include <fstream>

int WriteToParquet(const shared_ptr<arrow::Table> table, byte** outputData, int* outputSize, ParquetOptions options)
{
	const shared_ptr<arrow::io::BufferOutputStream> outputStream = parquet::CreateOutputStream();
	const shared_ptr<parquet::WriterProperties> writeProps = parquet::WriterProperties::Builder()
		.write_batch_size(options.WriteBatchSize)->compression(options.Compression)->build();
	const auto status = parquet::arrow::WriteTable(*table, arrow::default_memory_pool(), outputStream, table->num_rows(), writeProps);
	if (!status.ok())
	{
		cout << "Write parquet failed" << endl;
		return WriteToParquetError;
	}
	
	shared_ptr<arrow::Buffer> outputBuffer = move(outputStream->Finish()).ValueOrDie();
	const byte* bytes = reinterpret_cast<const byte*>(outputBuffer->data());
	*outputSize = outputBuffer->size();
	*outputData = new byte[*outputSize];
	memcpy(*outputData, bytes, *outputSize);

	return 0;
}

ParquetWriter::ParquetWriter()
{
}

ParquetWriter::ParquetWriter(const unordered_map<string, string>& schemaData)
{
	for (auto itr = schemaData.begin(); itr != schemaData.end(); itr ++)
	{
		_schemaManager.AddSchema(itr->first, itr->second);
	}
}

int ParquetWriter::RegisterSchema(const string& schemaKey, const string& schemaData)
{
	return _schemaManager.AddSchema(schemaKey, schemaData);
}

int ParquetWriter::Write(const string& resourceType, const char* inputJson, int inputLength, byte** outputData, int* outputLength)
{	
	auto schema = _schemaManager.GetSchema(resourceType);
	if (schema == nullptr)
	{
		return SchemaNotFound;
	}

	arrow::json::ParseOptions parseOptions = arrow::json::ParseOptions::Defaults();
	parseOptions.explicit_schema = move(schema);
	parseOptions.unexpected_field_behavior = _options.UnexpectedFieldBehavior;

	const auto bufferReader = make_shared<arrow::io::BufferReader>(reinterpret_cast<const uint8_t*>(inputJson), static_cast<int64_t>(inputLength));
    
	// ToDo: change to custom setting
	auto readOptions = arrow::json::ReadOptions::Defaults();
	readOptions.block_size = 1 << 30;
	readOptions.use_threads = true;
	arrow::Result<shared_ptr<arrow::json::TableReader>> tableReaderResult = arrow::json::TableReader::Make(arrow::default_memory_pool(), bufferReader, readOptions, parseOptions);
	
	if (!tableReaderResult.ok())
	{
		// cout << "Read json failed: " << tableReaderResult.status() << endl;
		return ReadInputJsonError;
	}		

	const shared_ptr<arrow::json::TableReader> tableReader = tableReaderResult.ValueOrDie();
	arrow::Result<shared_ptr<arrow::Table>> tableResult = move(tableReader->Read());

	if (!tableResult.ok())
	{
		// cout << "Make table failed: " << tableResult.status() << endl;
		return ReadInputJsonError;
	}

	const shared_ptr<arrow::Table> table = tableResult.ValueOrDie();
	return WriteToParquet(table, outputData, outputLength, _options);
}