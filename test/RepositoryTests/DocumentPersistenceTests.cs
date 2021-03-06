﻿using System.Collections.Generic;
using System.IO;
using Core.ZipManagementService;
using DataAccess;
using DataAccess.Contracts;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Newtonsoft.Json;
using NUnit.Framework;

namespace RepositoryTests
{
    public class DocumentPersistenceTests
    {
        private IDocumentPersistenceService _documentPersistenceService;

        private IConfiguration _configuration;

        private string bsonDocumentId;

        [SetUp]
        public void SetUp()
        {
            _configuration = Substitute.For<IConfiguration>();

            string dbPath = "../../../Data/Documents.db";

            _configuration[Core.Common.Constants.StorageConnectionString].Returns("FileName=../../../Data/Documents.db;Timeout=10; Journal=false;Mode=Exclusive");


            if (!File.Exists(dbPath))
            {
                Directory.CreateDirectory("../../../Data");
                File.Create(dbPath);
            }

            _documentPersistenceService = new DocumentPersistenceService(_configuration);
        }

        [Test]
        public void SaveJsonData_WhenInserted_ReturnsTrue()
        {
            bsonDocumentId = _documentPersistenceService.SaveDocument(JsonConvert.SerializeObject(GetDirectoryModel()));

            Assert.NotNull(bsonDocumentId);

            GetDocument_WhenIsValid_ReturnsTrue();
        }

        [Test]
        public void GetDocument_WhenIsValid_ReturnsTrue()
        {
            if (string.IsNullOrWhiteSpace(bsonDocumentId))
                SaveJsonData_WhenInserted_ReturnsTrue();

            var bsonDocument = _documentPersistenceService.GetDocument(bsonDocumentId);

            Assert.NotNull(bsonDocument);
        }

        private DirectoryModel GetDirectoryModel()
        {
            DirectoryModel _model = new DirectoryModel("root", "");

            _model.subItems.Add(new DirectoryModel("first item", "")
            {
                item = "data 2",
                subItems = new List<DirectoryModel>()
                {
                    new DirectoryModel("third item", "") {item = "data3"}
                }
            });
            return _model;
        }
    }
}