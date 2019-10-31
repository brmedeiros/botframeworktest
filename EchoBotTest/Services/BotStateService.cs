using EchoBotTest.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBotTest.Services
{
    public class BotStateService
    {
        public UserState UserState { get; }
        public ConversationState ConversationState { get; }

        public string UserProfileId { get; } = $"{nameof(BotStateService)}.UserProfile";
        public string ConversationDataId { get; } = $"{nameof(BotStateService)}.ConversationData";
        public string DialogStateID { get; } = $"{nameof(BotStateService)}.DialogState";

        public IStatePropertyAccessor<UserProfile> UserProfileAccessor { get; set; }
        public IStatePropertyAccessor<ConversationData> ConversationDataAccessor { get; set; }
        public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; set; }

        public BotStateService(UserState userState, ConversationState conversationState)
        {
            UserState = userState ?? throw new ArgumentNullException(nameof(userState));
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));

            InitializeAccesors();
        }

        private void InitializeAccesors()
        {
            //initialize user state
            UserProfileAccessor = UserState.CreateProperty<UserProfile>(UserProfileId);
            ConversationDataAccessor = ConversationState.CreateProperty<ConversationData>(ConversationDataId);
            DialogStateAccessor = ConversationState.CreateProperty<DialogState>(DialogStateID);
        }
    }
}
