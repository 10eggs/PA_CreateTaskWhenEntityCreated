
// <copyright file="PostOperationaccountCreate.cs" company="">
// Copyright (c) 2021 All Rights Reserved
// </copyright>
// <author></author>
// <date>10/17/2021 8:57:55 AM</date>
// <summary>Implements the PostOperationaccountCreate Plugin.</summary>
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
// </auto-generated>

using System;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace CreateTaskWhenEntityCreated.CreateTaskWhenEntityCreated
{

    /// <summary>
    /// PostOperationaccountCreate Plugin.
    /// </summary>    
    public class PostOperationaccountCreate: PluginBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostOperationaccountCreate"/> class.
        /// </summary>
        /// <param name="unsecure">Contains public (unsecured) configuration information.</param>
        /// <param name="secure">Contains non-public (secured) configuration information. 
        /// When using Microsoft Dynamics 365 for Outlook with Offline Access, 
        /// the secure string is not passed to a plug-in that executes while the client is offline.</param>
        public PostOperationaccountCreate(string unsecure, string secure)
            : base(typeof(PostOperationaccountCreate))
        {
            
           // TODO: Implement your custom configuration handling.
        }


        /// <summary>
        /// Main entry point for he business logic that the plug-in is to execute.
        /// </summary>
        /// <param name="localContext">The <see cref="LocalPluginContext"/> which contains the
        /// <see cref="IPluginExecutionContext"/>,
        /// <see cref="IOrganizationService"/>
        /// and <see cref="ITracingService"/>
        /// </param>
        /// <remarks>
        /// For improved performance, Microsoft Dynamics 365 caches plug-in instances.
        /// The plug-in's Execute method should be written to be stateless as the constructor
        /// is not called for every invocation of the plug-in. Also, multiple system threads
        /// could execute the plug-in at the same time. All per invocation state information
        /// is stored in the context. This means that you should not use global variables in plug-ins.
        /// </remarks>
        protected override void ExecuteCdsPlugin(ILocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new InvalidPluginExecutionException(nameof(localContext));
            }           
            // Obtain the tracing service
            ITracingService tracingService = localContext.TracingService;

            try
            {
                // Obtain the execution context from the service provider.  
                IPluginExecutionContext context = (IPluginExecutionContext)localContext.PluginExecutionContext;

                // Obtain the organization service reference for web service calls.  
                IOrganizationService service = localContext.CurrentUserService;

                // TODO: Implement your custom Plug-in business logic.
                // The InputParameters collection contains all the data passed in the message request.  
                if (context.InputParameters.Contains("Target") &&
                    context.InputParameters["Target"] is Entity)
                {
                    // Obtain the target entity from the input parameters.  
                    Entity entity = (Entity)context.InputParameters["Target"];

                    // Obtain the organization service reference which you will need for  
                    // web service calls.  
                    try
                    {
                        // Plug-in business logic goes here.  
                        // Create a task activity to follow up with the account customer in 7 days. 
                        Entity followup = new Entity("task");

                        followup["subject"] = "New car created";
                        followup["description"] =
                            "New car has been created. This task has been created by custom plug-in.";
                        followup["scheduledstart"] = DateTime.Now.AddDays(7);
                        followup["scheduledend"] = DateTime.Now.AddDays(7);
                        followup["category"] = context.PrimaryEntityName;

                        // Refer to the account in the task activity.
                        if (context.OutputParameters.Contains("id"))
                        {
                            Guid regardingobjectid = new Guid(context.OutputParameters["id"].ToString());
                            //change for account and verify if it works
                            string regardingobjectidType = "account";

                            followup["regardingobjectid"] =
                            new EntityReference(regardingobjectidType, regardingobjectid);
                        }

                        // Create the task in Microsoft Dynamics CRM.
                        tracingService.Trace("FollowupPlugin: Creating the task activity.");
                        service.Create(followup);
                    }

                    catch (FaultException<OrganizationServiceFault> ex)
                    {
                        throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin.", ex);
                    }

                    catch (Exception ex)
                    {
                        tracingService.Trace("FollowUpPlugin: {0}", ex.ToString());
                        throw;
                    }
                }


            }
            // Only throw an InvalidPluginExecutionException. Please Refer https://go.microsoft.com/fwlink/?linkid=2153829.
            catch (Exception ex)
            {
                tracingService?.Trace("An error occurred executing Plugin CreateTaskWhenEntityCreated.CreateTaskWhenEntityCreated.PostOperationcrd9a_carCreate : {0}", ex.ToString());
                throw new InvalidPluginExecutionException("An error occurred executing Plugin CreateTaskWhenEntityCreated.CreateTaskWhenEntityCreated.PostOperationcrd9a_carCreate .", ex);
            }
        }
    }
}
