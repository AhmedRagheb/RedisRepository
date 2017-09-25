using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedisRepository.Test
{
    [TestClass]
    public class RedisClientTest
    {
        private RedisClient _redisClient;
        [TestInitialize]
        public void Initialize()
        {
            _redisClient = new RedisClient();
        }

        protected virtual TimeSpan GetDefaultTimeSpan()
        {
            return new TimeSpan(1, 0, 0, 0);
        }

        [TestMethod]
        public void AddArticlesTest()
        {
            var article = new ArticleDocument
                                          {
                                              Id = 1,
                                              Title = "Test Article 1",
                                              Body = "Body",
                                              CreatedOn = DateTime.Now,
                                              IsPublished = true
                                          }; 

            var added = _redisClient.Add("article:" + article.Id, article, GetDefaultTimeSpan());
            Assert.IsTrue(added);
        }

        [TestMethod]
        public void UpdateArticlesTest()
        {
            var article = new ArticleDocument
            {
                Id = 1,
                Title = "Test Article 1",
                Body = "Body 1",
                CreatedOn = DateTime.Now,
                IsPublished = true
            };

            var added = _redisClient.Update("article:" + article.Id, article);
            Assert.IsTrue(added);
        }

        [TestMethod]
        public void GetArticleTest()
        {
            var article = _redisClient.Get<ArticleDocument>("article:1");
            Assert.AreEqual("Test Article 1", article.Title);
        }

        [TestMethod]
        public void GetAllArticlesTest(int id)
        {
            var allArticles =  _redisClient.GetList<ArticleDocument>("article:*");
            Assert.AreEqual(1, allArticles.Count);
        }


        [TestMethod]
        public bool RemoveArticlesTest()
        {
            return _redisClient.Remove("article:1");
        }
        
        


    }
}
