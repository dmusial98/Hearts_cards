using System.Globalization;
using System.Runtime.InteropServices;

namespace HeartsServer.GameLogic.Consts
{
    public class NumbersConsts
    {
        public const int CARDS_NUMBER_CONST = 52;
        public const int PLAYERS_NUMBER_CONST = 4;
        public const int CARDS_FOR_PLAYER_CONST = 13;
        public const int QUEEN_SPADE_POINTS_CONST = 13;
        public const int TRICKS_NUMBER_IN_ROUND_CONST = 13;
        public const int FULL_POINTS_IN_ROUND_CONST = 26;
    }

    public class LogCodesConsts
    {
        public const string USER_CONNECTED_CODE_CONST = "C1";
        public const string USER_CLICKED_START_CONST = "C2";
        public const string GAME_STARTED_CODE_CONST = "C3";
        public const string PLAYERS_GOT_CARDS_CODE_CONST = "C4";
        public const string PLAYER_GAVE_CARDS_CODE_CONST = "C5";
        public const string PLAYER_RECEIVED_CARDS_CODE_CONST = "C6";
        public const string PLAYER_THREW_CARD_CODE_CONST = "C7";
        public const string TRICK_CODE_CONST = "C8";
        public const string PLAYERS_POINTS_IN_ROUND_CODE_CONST = "C9";
        public const string PLAYERS_POINTS_AFTER_ROUND_CODE_CONST = "C10";
        public const string PLAYERS_PLACES_AFTER_ROUND_CODE_CONST = "C11";
        public const string PLAYERS_CARDS_CODE_CONST = "C12";
        public const string USER_SEND_MESSAGE_CODE_CONST = "C13";
        public const string ROUND_STARTED_CODE_CONST = "C14";
        public const string TRICK_STARTED_CODE_CONST = "C15";

        public static readonly CultureInfo POLISH_CULTURE_INFO = new("pl-PL");
        
        private static string newLine;
        public static string NEW_LINE
        {
            get
            {
                if (String.IsNullOrEmpty(newLine))
                {
                    if (RuntimeInformation.IsOSPlatform((OSPlatform.Linux)))
                        newLine = "\n";
                    else
                        newLine = "\r\n";
                }

                return newLine;
            }
        }

private static string slashDirectory;
        public static string SLASH_DIRECTORY
        {
            get
            {
                if (String.IsNullOrEmpty(slashDirectory))
                {
                    if (RuntimeInformation.IsOSPlatform((OSPlatform.Linux)))
                        slashDirectory = "/";
                    else
                        slashDirectory = "\\";
                }

                return slashDirectory;
            }
        }
    }
}
