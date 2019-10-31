// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.5.0

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EchoBotTest.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using EchoBotTest.Models;
//using Microsoft.Bot.Builder.Dialogs;

namespace EchoBotTest.Bots
{
    public class EchoBot : ActivityHandler
    {
        private readonly BotStateService _botStateService;

        public EchoBot(BotStateService botStateService)
        {
            _botStateService = botStateService ?? throw new ArgumentNullException(nameof(botStateService));
        }

        private async Task GetName(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            //GetAsync either retrieves UserProfile data from botStateService or create new instance if there isnt on yet
            UserProfile userProfile = await _botStateService.UserProfileAccessor.GetAsync(turnContext, () => new UserProfile());
            ConversationData conversationData = await _botStateService.ConversationDataAccessor.GetAsync(turnContext, () => new ConversationData());

            if (!string.IsNullOrEmpty(userProfile.Name))
            {
                await turnContext.SendActivityAsync(MessageFactory.Text($"Hi {userProfile.Name}, how can I help you?"), cancellationToken);
            }
            else
            {
                if (conversationData.PromptedUserForName )
                {
                    userProfile.Name = turnContext.Activity.Text?.Trim();

                    await turnContext.SendActivityAsync(MessageFactory.Text($"Thanks {userProfile.Name}, how can I help you?"), cancellationToken);

                    conversationData.PromptedUserForName = false;
                }

                else
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"What is your name?"), cancellationToken);
                    conversationData.PromptedUserForName = true;
                }

                await _botStateService.UserProfileAccessor.SetAsync(turnContext, userProfile);
                await _botStateService.ConversationDataAccessor.SetAsync(turnContext, conversationData);

                await _botStateService.UserState.SaveChangesAsync(turnContext);
                await _botStateService.ConversationState.SaveChangesAsync(turnContext);
            }

        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await GetName(turnContext, cancellationToken);
            //string botResponse;

            //switch (turnContext.Activity.Text.ToLower())
            //{
            //    case "hi!": botResponse = "Hello!"; break;
            //    case "hello!": botResponse = "Hi!"; break;
            //    default: botResponse = turnContext.Activity.Text; break;
            //}
            //await turnContext.SendActivityAsync(MessageFactory.Text($"Echo: {botResponse}"), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and welcome!"), cancellationToken);
                }
            }
        }
    }
}
