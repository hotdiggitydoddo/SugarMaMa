using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SugarMaMa.API.Helpers
{
    /// <summary>
    /// Automatically responds with validation messages if ModelState validation fails. Additionally valides the required attribute placed on a
    /// parameter directly in the actionmethod
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //validate attributes placed on action method paramemters themselves
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var parameters = descriptor.MethodInfo.GetParameters();

                foreach (var parameter in parameters)
                {
                    var arg = context.ActionArguments.SingleOrDefault(x => x.Key == parameter.Name);
                    var argumentValue = arg.Equals(new KeyValuePair<string, object>())
                        ? null : arg.Value;

                    EvaluateValidationAttributes(parameter, argumentValue, context.ModelState);
                }
            }

            if (!context.ModelState.IsValid)
            {
                var validationErrors = context.ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)
                    ).Select(x => new
                    {
                        field = x.Key,
                        error = x.Value
                    });

                var objectRes = new ObjectResult(new { validationErrors })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

                //flush the response
                context.Result = objectRes;
            }
        }

        private void EvaluateValidationAttributes(ParameterInfo parameter, object argument, ModelStateDictionary modelState)
        {
            var validationAttributes = parameter.CustomAttributes;

            foreach (var attributeData in validationAttributes)
            {
                var attributeInstance = parameter.GetCustomAttribute(attributeData.AttributeType);

                var validationAttribute = attributeInstance as ValidationAttribute;

                if (validationAttribute != null)
                {
                    var isValid = validationAttribute.IsValid(argument);
                    if (!isValid)
                    {
                        modelState.AddModelError(parameter.Name, validationAttribute.FormatErrorMessage(parameter.Name));
                    }
                }
            }
        }
    }
}
