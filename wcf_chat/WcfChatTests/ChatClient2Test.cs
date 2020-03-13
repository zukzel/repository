using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Word = Microsoft.Office.Interop.Word;
using System.Data.SqlClient;

namespace WcfChatTests
{
    [TestClass]
    public class ChatClient2Test
    {
        [TestMethod]
        public void strDBconnect()
        {
            //arrange

            string Server = "USER3-ПК";

            string Database = "test002";

            string expected = "Data Source = USER3-ПК; Initial Catalog = test002; Integrated Security = True";

            //act

            ChatClient2.Teacher teacher = new ChatClient2.Teacher();

            string actual = teacher.strDBconnect(Server, Database);

            //assert

            Assert.AreEqual(expected, actual);
        }

        private readonly string TemplateFileName = @"c:\test.docx";

        [TestMethod]
        public void SelectID()
        {
            //arrange

            string query = "SELECT Фамилия FROM Пользователи WHERE idПользователя = 1";

            string expected = "Козырчиков";

            //act

            ChatClient2.Teacher teacher = new ChatClient2.Teacher();

            string actual = teacher.SelectID(query);

            //assert

            Assert.AreEqual(expected, actual);
        }
    }
}
