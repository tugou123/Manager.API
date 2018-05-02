
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace Manager.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerAddRequirHeaderParmeter:IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }
            operation.parameters
                .Add(new Parameter() {
                    name="Token",
                    @in="header",
                    type="string",
                    @default="",
                    required=false,
                    description="登陆授权Token"

                });
        }
    }


}