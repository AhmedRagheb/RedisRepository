using System;

namespace RedisRepository.Test
{
    public class ArticleDocument
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsPublished { get; set; }
    }
}
