﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portalum.Zvt.Helpers;
using Portalum.Zvt.Models;
using Portalum.Zvt.Repositories;
using System.Diagnostics;
using System.Text;

namespace Portalum.Zvt.UnitTest
{
    [TestClass]
    public class PrintLineTest
    {
        private bool _lineReceived = false;

        [TestInitialize]
        public void TestInitialize()
        {
            this._lineReceived = false;
        }

        private ReceiveHandler GetReceiveHandler()
        {
            IErrorMessageRepository errorMessageRepository = new EnglishErrorMessageRepository();
            IIntermediateStatusRepository intermediateStatusRepository = new EnglishIntermediateStatusRepository();

            var logger = LoggerHelper.GetLogger();
            var encoding = Encoding.GetEncoding(437);

            return new ReceiveHandler(logger.Object, encoding, errorMessageRepository, intermediateStatusRepository);
        }

        [TestMethod]
        public void ProcessData_LineReceived1_Successful()
        {
            var hexLines = new string[]
            {
                "06-D1-14-40-2A-2A-20-4B-41-53-53-45-4E-53-43-48-4E-49-54-54-20-2A-2A",
                "06-D1-0E-40-54-45-53-54-20-50-6F-72-74-61-6C-75-6D",
                "06-D1-09-40-52-49-45-44-47-20-35-30",
                "06-D1-0E-40-36-38-35-30-20-44-4F-52-4E-42-49-52-4E",
                "06-D1-01-40",
                "06-D1-01-40",
                "06-D1-02-00-20",
                "06-D1-19-00-54-65-72-6D-69-6E-61-6C-2D-49-44-3A-20-20-20-20-32-38-30-30-34-38-36-39",
                "06-D1-14-00-31-38-2E-31-30-2E-32-30-32-31-20-32-32-3A-35-35-3A-35-30",
                "06-D1-02-00-20",
                "06-D1-19-00-20-20-20-20-20-20-20-20-41-6E-7A-61-68-6C-20-20-20-20-20-20-20-45-55-52",
                "06-D1-17-00-56-69-73-61-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20",
                "06-D1-19-00-20-20-20-20-20-20-20-20-20-20-20-20-20-30-20-20-20-20-20-20-30-2C-30-30",
                "06-D1-17-00-56-20-50-41-59-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20",
                "06-D1-19-00-20-20-20-20-20-20-20-20-20-20-20-20-20-30-20-20-20-20-20-20-30-2C-30-30",
                "06-D1-17-00-4D-61-73-74-65-72-63-61-72-64-20-20-20-20-20-20-20-20-20-20-20-20",
                "06-D1-19-00-20-20-20-20-20-20-20-20-20-20-20-20-20-30-20-20-20-20-20-20-30-2C-30-30",
                "06-D1-17-00-4D-61-65-73-74-72-6F-2F-44-4D-43-20-41-54-20-20-20-20-20-20-20-20",
                "06-D1-19-00-20-20-20-20-20-20-20-20-20-20-20-20-20-30-20-20-20-20-20-20-30-2C-30-30",
                "06-D1-17-00-2D-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20",
                "06-D1-19-00-20-20-20-20-20-20-20-20-20-20-20-20-20-30-20-20-20-20-20-20-30-2C-30-30",
                "06-D1-19-40-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D",
                "06-D1-19-00-47-65-73-61-6D-74-20-20-20-20-20-20-20-30-20-20-20-20-20-20-30-2C-30-30",
                "06-D1-02-00-20",
                "06-D1-19-40-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D",
                "06-D1-19-00-56-69-73-61-20-20-20-20-20-20-20-20-20-20-20-20-23-3A-30-30-30-34-39-39",
                "06-D1-19-00-56-55-3A-20-20-20-20-20-20-20-20-20-20-20-20-31-30-30-34-36-31-37-36-33",
                "06-D1-19-00-56-20-50-41-59-20-20-20-20-20-20-20-20-20-20-20-23-3A-30-30-30-34-39-39",
                "06-D1-19-00-56-55-3A-20-20-20-20-20-20-20-20-20-20-20-20-31-30-30-34-36-31-37-36-33",
                "06-D1-19-00-4D-61-73-74-65-72-63-61-72-64-20-20-20-20-20-20-23-3A-30-30-30-34-39-39",
                "06-D1-19-00-56-55-3A-20-20-20-20-20-20-20-20-20-20-20-20-31-30-30-34-36-31-37-36-33",
                "06-D1-19-00-4D-61-65-73-74-72-6F-2F-44-4D-43-20-41-54-20-20-23-3A-30-30-30-34-39-39",
                "06-D1-19-00-56-55-3A-20-20-20-20-20-20-20-20-20-20-20-20-31-30-30-34-36-31-37-36-33",
                "06-D1-19-00-2D-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-23-3A-30-30-30-34-39-39",
                "06-D1-19-00-56-55-3A-20-20-20-20-20-20-20-20-20-20-20-20-31-30-30-34-36-31-37-36-33",
                "06-D1-19-40-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D",
                "06-D1-02-00-20",
                "06-D1-02-00-20",
                "06-D1-02-00-20",
                "06-D1-02-00-20",
                "06-D1-01-81",
                "06-0F-02-27-E0"
            };

            var receiveHandler = this.GetReceiveHandler();
            receiveHandler.LineReceived += ReceiveHandlerLineReceived;

            foreach (var hexLine in hexLines)
            {
                var data = ByteHelper.HexToByteArray(hexLine);
                var canProcess = receiveHandler.ProcessData(data);
            }

            receiveHandler.LineReceived -= ReceiveHandlerLineReceived;

            Assert.IsTrue(this._lineReceived, "No lines received");
        }

        [TestMethod]
        public void ProcessData_LineReceivedCardCompleteWithTlv_Successful()
        {
            var hexLines = new string[]
            {
                "06-D1-19-40-2A-2A-20-48-C4-4E-44-4C-45-52-42-45-4C-45-47-20-2A-2A-06-04-1F-07-01-01",
                "06-D1-14-40-54-45-53-54-20-50-6F-72-74-61-6C-75-6D-06-04-1F-07-01-01",
                "06-D1-0F-40-52-49-45-44-47-20-35-30-06-04-1F-07-01-01",
                "06-D1-14-40-36-38-35-30-20-44-4F-52-4E-42-49-52-4E-06-04-1F-07-01-01",
                "06-D1-07-40-06-04-1F-07-01-01",
                "06-D1-07-40-06-04-1F-07-01-01",
                "06-D1-08-00-20-06-04-1F-07-01-01",
                "06-D1-0E-40-56-45-52-4B-41-55-46-06-04-1F-07-01-01",
                "06-D1-1F-00-30-32-2E-31-31-2E-32-30-32-31-20-20-20-20-20-20-30-38-3A-33-35-3A-33-32-06-04-1F-07-01-01",
                "06-D1-1F-00-54-65-72-6D-69-6E-61-6C-2D-49-44-3A-20-20-20-20-32-38-30-30-34-38-36-39-06-04-1F-07-01-01",
                "06-D1-1F-00-56-55-2D-4E-72-3A-20-20-20-20-20-20-20-20-20-31-30-30-34-36-31-37-36-33-06-04-1F-07-01-01",
                "06-D1-1F-00-42-65-6C-65-67-2D-4E-72-3A-20-20-20-20-20-20-20-20-30-30-2F-30-35-30-32-06-04-1F-07-01-01",
                "06-D1-1F-00-54-72-61-63-65-2D-4E-72-3A-20-20-20-20-20-20-20-20-20-30-30-31-30-34-34-06-04-1F-07-01-01",
                "06-D1-15-40-4D-61-65-73-74-72-6F-2F-44-4D-43-20-41-54-06-04-1F-07-01-01",
                "06-D1-17-40-44-65-62-69-74-20-4D-61-73-74-65-72-63-61-72-64-06-04-1F-07-01-01",
                "06-D1-1F-00-41-49-44-3A-20-20-20-20-20-20-41-30-30-30-30-30-30-30-30-34-31-30-31-30-06-04-1F-07-01-01",
                "06-D1-1F-00-43-72-79-70-74-6F-3A-20-45-46-34-46-41-36-38-38-31-38-44-42-37-37-32-30-06-04-1F-07-01-01",
                "06-D1-1F-00-4E-72-3A-20-20-2A-2A-2A-2A-20-2A-2A-2A-2A-20-2A-2A-2A-2A-20-34-38-32-31-06-04-1F-07-01-01",
                "06-D1-1F-00-41-62-6C-61-75-66-64-61-74-75-6D-3A-20-20-20-20-20-20-20-31-32-2F-32-35-06-04-1F-07-01-01",
                "06-D1-1F-00-53-45-51-3A-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-30-30-31-06-04-1F-07-01-01",
                "06-D1-1A-00-45-2E-4D-6F-64-65-28-49-29-20-53-56-43-32-30-36-20-20-20-06-04-1F-07-01-01",
                "06-D1-0E-40-42-45-5A-41-48-4C-54-06-04-1F-07-01-01",
                "06-D1-15-00-47-45-4E-2E-4E-52-2E-3A-39-37-39-35-37-32-06-04-1F-07-01-01",
                "06-D1-1F-00-42-45-54-52-41-47-3A-20-20-45-55-52-20-20-20-20-20-20-20-20-32-2C-30-30-06-04-1F-07-01-01",
                "06-D1-1F-00-20-20-20-20-20-20-20-20-20-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-06-04-1F-07-01-01",
                "06-D1-08-00-20-06-04-1F-07-01-01",
                "06-D1-07-40-06-04-1F-07-01-01",
                "06-D1-07-40-06-04-1F-07-01-01",
                "06-D1-07-40-06-04-1F-07-01-01",
                "06-D1-08-00-20-06-04-1F-07-01-01",
                "06-D1-08-00-20-06-04-1F-07-01-01",
                "06-D1-08-00-20-06-04-1F-07-01-01",
                "06-D1-08-00-20-06-04-1F-07-01-01",
                "06-D1-07-81-06-04-1F-07-01-01",
                "06-D1-18-40-2A-2A-20-4B-55-4E-44-45-4E-42-45-4C-45-47-20-2A-2A-06-04-1F-07-01-02",
                "06-D1-14-40-54-45-53-54-20-50-6F-72-74-61-6C-75-6D-06-04-1F-07-01-01",
                "06-D1-0F-40-52-49-45-44-47-20-35-30-06-04-1F-07-01-02",
                "06-D1-14-40-36-38-35-30-20-44-4F-52-4E-42-49-52-4E-06-04-1F-07-01-02",
                "06-D1-07-40-06-04-1F-07-01-02",
                "06-D1-07-40-06-04-1F-07-01-02",
                "06-D1-08-00-20-06-04-1F-07-01-02",
                "06-D1-0E-40-56-45-52-4B-41-55-46-06-04-1F-07-01-02",
                "06-D1-1F-00-30-32-2E-31-31-2E-32-30-32-31-20-20-20-20-20-20-30-38-3A-33-35-3A-33-32-06-04-1F-07-01-02",
                "06-D1-1F-00-54-65-72-6D-69-6E-61-6C-2D-49-44-3A-20-20-20-20-32-38-30-30-34-38-36-39-06-04-1F-07-01-02",
                "06-D1-1F-00-56-55-2D-4E-72-3A-20-20-20-20-20-20-20-20-20-31-30-30-34-36-31-37-36-33-06-04-1F-07-01-02",
                "06-D1-1F-00-42-65-6C-65-67-2D-4E-72-3A-20-20-20-20-20-20-20-20-30-30-2F-30-35-30-32-06-04-1F-07-01-02",
                "06-D1-1F-00-54-72-61-63-65-2D-4E-72-3A-20-20-20-20-20-20-20-20-20-30-30-31-30-34-34-06-04-1F-07-01-02",
                "06-D1-17-40-44-65-62-69-74-20-4D-61-73-74-65-72-63-61-72-64-06-04-1F-07-01-02",
                "06-D1-1F-00-41-49-44-3A-20-20-20-20-20-20-41-30-30-30-30-30-30-30-30-34-31-30-31-30-06-04-1F-07-01-02",
                "06-D1-1F-00-43-72-79-70-74-6F-3A-20-45-46-34-46-41-36-38-38-31-38-44-42-37-37-32-30-06-04-1F-07-01-02",
                "06-D1-1F-00-4E-72-3A-20-20-2A-2A-2A-2A-20-2A-2A-2A-2A-20-2A-2A-2A-2A-20-34-38-32-31-06-04-1F-07-01-02",
                "06-D1-1F-00-41-62-6C-61-75-66-64-61-74-75-6D-3A-20-20-20-20-20-20-20-31-32-2F-32-35-06-04-1F-07-01-02",
                "06-D1-1F-00-53-45-51-3A-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-20-30-30-31-06-04-1F-07-01-02",
                "06-D1-1A-00-45-2E-4D-6F-64-65-28-49-29-20-53-56-43-32-30-36-20-20-20-06-04-1F-07-01-02",
                "06-D1-0E-40-42-45-5A-41-48-4C-54-06-04-1F-07-01-02",
                "06-D1-15-00-47-45-4E-2E-4E-52-2E-3A-39-37-39-35-37-32-06-04-1F-07-01-02",
                "06-D1-1F-00-42-45-54-52-41-47-3A-20-20-45-55-52-20-20-20-20-20-20-20-20-32-2C-30-30-06-04-1F-07-01-02",
                "06-D1-1F-00-20-20-20-20-20-20-20-20-20-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-3D-06-04-1F-07-01-02",
                "06-D1-08-00-20-06-04-1F-07-01-02",
                "06-D1-1F-40-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-06-04-1F-07-01-02",
                "06-D1-12-40-56-49-45-4C-45-4E-20-44-41-4E-4B-06-04-1F-07-01-02",
                "06-D1-1F-40-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-2D-06-04-1F-07-01-02",
                "06-D1-08-00-20-06-04-1F-07-01-02",
                "06-D1-08-00-20-06-04-1F-07-01-02",
                "06-D1-08-00-20-06-04-1F-07-01-02",
                "06-D1-08-00-20-06-04-1F-07-01-02",
                "06-D1-07-81-06-04-1F-07-01-02"
            };

            var receiveHandler = this.GetReceiveHandler();
            receiveHandler.LineReceived += ReceiveHandlerLineReceived;

            foreach (var hexLine in hexLines)
            {
                var data = ByteHelper.HexToByteArray(hexLine);
                receiveHandler.ProcessData(data);
            }

            receiveHandler.LineReceived -= ReceiveHandlerLineReceived;

            Assert.IsTrue(this._lineReceived, "No lines received");
        }

        [TestMethod]
        public void ProcessData_CorruptLinesReceivedPackageToLarge_Successful()
        {
            var hexLines = new string[]
            {
                "06-D1-10-40-2A-2A-20-4B-41-53-53-45-4E-53-43-48-4E-49-54-54-20-2A-2A" //Invalid length, data to large
            };

            var receiveHandler = this.GetReceiveHandler();
            receiveHandler.LineReceived += ReceiveHandlerLineReceived;

            foreach (var hexLine in hexLines)
            {
                var data = ByteHelper.HexToByteArray(hexLine);
                var processDataState = receiveHandler.ProcessData(data);
                Assert.AreEqual(processDataState, ProcessDataState.CannotProcess);
            }

            receiveHandler.LineReceived -= ReceiveHandlerLineReceived;

            Assert.IsFalse(this._lineReceived, "Lines received");
        }

        [TestMethod]
        public void ProcessData_CorruptLinesReceivedFragment_Successful()
        {
            var hexLines = new string[]
            {
                "06-D1-18-40-2A-2A-20-4B-41-53-53-45-4E-53-43-48-4E-49-54-54-20-2A-2A" //Invalid length, data too short
            };

            var receiveHandler = this.GetReceiveHandler();
            receiveHandler.LineReceived += ReceiveHandlerLineReceived;

            foreach (var hexLine in hexLines)
            {
                var data = ByteHelper.HexToByteArray(hexLine);
                var processDataState = receiveHandler.ProcessData(data);
                Assert.AreEqual(processDataState, ProcessDataState.WaitForMoreData);
            }

            receiveHandler.LineReceived -= ReceiveHandlerLineReceived;

            Assert.IsFalse(this._lineReceived, "Lines received");
        }

        private void ReceiveHandlerLineReceived(PrintLineInfo printLineInfo)
        {
            Trace.WriteLine(printLineInfo.Text);
            this._lineReceived = true;
        }
    }
}
