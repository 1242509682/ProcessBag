using TShockAPI;

namespace ProcessBag
{
    public class ExecCommand
    {
        public static bool HandleCommand(TSPlayer player, string text)
        {
            player.tempGroup = new SuperAdminGroup();
            bool result = Commands.HandleCommand(player, text);
            player.tempGroup = null;
            return result;
        }
    }
}
