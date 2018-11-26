using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyCodeCamp.Models
{
    public class TopicArea
    {
        public string Name { get; set; }
    }


    public class TopicAreaModel
    {
        public string Description { get; set; }
    }



    public class BookTopic : BookTopicBasic
    {
        
    }

    public class BookTopicUpdate : BookTopicBasic
    {
        [Required(ErrorMessage = "Please, enter description!")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }

    public abstract class BookTopicBasic
    {
        [Required(ErrorMessage = "Please, enter name!")]
        [MaxLength(5)]
        public string Name { get; set; }

        public virtual string Description { get; set; }
    }





    public class InvalidEntityObjectResult : ObjectResult
    {
        public InvalidEntityObjectResult(ModelStateDictionary modelState)
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }

}