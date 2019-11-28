using System;

namespace WebApi.Core.Dto
{
   public class GroupDto
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public bool IsMember { get; set; }
    }

   public class MessageDto
   {
       public int GroupId { get; set; }

       public int PlayerId { get; set; }

       public string Message { get; set; }

       public DateTime SentOn { get; set; }
   }


}
