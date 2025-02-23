using Scellecs.Morpeh;

namespace EscapeRooms.Initializers
{
    public static class InitializersExecutionOrder
    {
        public static void AddInitializersSequence(SystemsGroup group)
        {
            SettingsBlock(group);
        }

        private static void SettingsBlock(SystemsGroup group)
        {
            group.AddInitializer(new SettingsInitializer());
        }
    }
}