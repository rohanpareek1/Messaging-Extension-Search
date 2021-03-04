// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using TeamsMessagingExtensionsSearch.Models;
using System;
using AdaptiveCards;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class TeamsMessagingExtensionsSearchBot : TeamsActivityHandler
    {
        protected override async Task<MessagingExtensionResponse> OnTeamsMessagingExtensionQueryAsync(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionQuery query, CancellationToken cancellationToken)
        {
            var text = query?.Parameters?[0]?.Value as string ?? string.Empty;

            var packages = await FindPackages(text);

            // We take every row of the results and wrap them in cards wrapped in in MessagingExtensionAttachment objects.
            // The Preview is optional, if it includes a Tap, that will trigger the OnTeamsMessagingExtensionSelectItemAsync event back on this bot.
            var attachments = packages.Select(package => {
                var previewCard = new ThumbnailCard { Title = package.AssignedUser, Tap = new CardAction { Type = "invoke", Value = package } };

                var attachment = new MessagingExtensionAttachment
                {
                    ContentType = HeroCard.ContentType,
                    Content = new HeroCard { Title = package.AssignedUser },
                    Preview = previewCard.ToAttachment()
                };

                return attachment;
            }).ToList();

            // The list of MessagingExtensionAttachments must we wrapped in a MessagingExtensionResult wrapped in a MessagingExtensionResponse.
            return new MessagingExtensionResponse
            {
                ComposeExtension = new MessagingExtensionResult
                {
                    Type = "result",
                    AttachmentLayout = "list",
                    Attachments = attachments
                }
            };
        }


        protected override Task<MessagingExtensionResponse> OnTeamsMessagingExtensionSelectItemAsync(ITurnContext<IInvokeActivity> turnContext, JObject query, CancellationToken cancellationToken)
        {
            // The Preview card's Tap should have a Value property assigned, this will be returned to the bot in this event. 
            var lead = query.ToObject<NameData>();


            //We take every row of the results and wrap them in cards wrapped in in MessagingExtensionAttachment objects.
            // The Preview is optional, if it includes a Tap, that will trigger the OnTeamsMessagingExtensionSelectItemAsync event back on this bot.
            var card = new ThumbnailCard
            {
                Title = $"{lead.AssignedUser}",
                Subtitle = lead.Contact,
                Text = Convert.ToString(lead.Converted),


                Buttons = new List<CardAction>
                {
                    //new CardAction { Type = ActionTypes.OpenUrl, Title = "Nuget Package", Value = $"https://www.nuget.org/packages/{packageId}" },
                    new CardAction { Type = ActionTypes.OpenUrl, Title = "Chat", Value = lead.Website },
                },
            };


            var attachment = new MessagingExtensionAttachment
            {
                ContentType = ThumbnailCard.ContentType,
                Content = card,
            };


            return Task.FromResult(new MessagingExtensionResponse
            {
                ComposeExtension = new MessagingExtensionResult
                {
                    Type = "result",
                    AttachmentLayout = "list",
                    Attachments = new List<MessagingExtensionAttachment> { attachment }
                }
            });

        }


        // Generate a set of substrings to illustrate the idea of a set of results coming back from a query. 
        private async Task<IEnumerable<NameData>> FindPackages(string text)
        {
            var namedata = File.ReadAllText(@"E:\celebal\bot notes\Msg ext-Search\MessagingExtensionwithSearch\MessagingExtensionwithSearch\Data.json");
            var data = JsonConvert.DeserializeObject<IEnumerable<NameData>>(namedata);
            List<NameData> result = new List<NameData>();
            foreach (var name in data)
            {
                if (name.AssignedUser.Contains(text))
                {
                    result.Add(name);
                }
            }
            return result;
        }
    }
}
