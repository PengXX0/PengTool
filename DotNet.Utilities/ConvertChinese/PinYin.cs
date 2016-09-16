using System;
using System.Globalization;
using System.Text;

namespace DotNet.Utilities.ConvertChinese
{
    public class PinYin
    {
        public string GetFirstLetter(string chineseCharacter)
        {
            const string lsSecondEng = "CJWGNSPGCGNESYPBTYYZDXYKYGTDJNNJQMBSGZSCYJSYYQPGKBZGYCYWJKGKLJSWKPJQHYTWDDZLSGMRYPYWWCCKZNKYDGTTNGJEYKKZYTCJNMCYLQLYPYQFQRPZSLWBTGKJFYXJWZLTBNCXJJJJZXDTTSQZYCDXXHGCKBPHFFSSWYBGMXLPBYLLLHLXSPZMYJHSOJNGHDZQYKLGJHSGQZHXQGKEZZWYSCSCJXYEYXADZPMDSSMZJZQJYZCDJZWQJBDZBXGZNZCPWHKXHQKMWFBPBYDTJZZKQHYLYGXFPTYJYYZPSZLFCHMQSHGMXXSXJJSDCSBBQBEFSJYHWWGZKPYLQBGLDLCCTNMAYDDKSSNGYCSGXLYZAYBNPTSDKDYLHGYMYLCXPYCJNDQJWXQXFYYFJLEJBZRXCCQWQQSBNKYMGPLBMJRQCFLNYMYQMSQTRBCJTHZTQFRXQ" +
                                         "HXMJJCJLXQGJMSHZKBSWYEMYLTXFSYDSGLYCJQXSJNQBSCTYHBFTDCYZDJWYGHQFRXWCKQKXEBPTLPXJZSRMEBWHJLBJSLYYSMDXLCLQKXLHXJRZJMFQHXHWYWSBHTRXXGLHQHFNMNYKLDYXZPWLGGTMTCFPAJJZYLJTYANJGBJPLQGDZYQYAXBKYSECJSZNSLYZHZXLZCGHPXZHZNYTDSBCJKDLZAYFMYDLEBBGQYZKXGLDNDNYSKJSHDLYXBCGHXYPKDJMMZNGMMCLGWZSZXZJFZNMLZZTHCSYDBDLLSCDDNLKJYKJSYCJLKOHQASDKNHCSGANHDAASHTCPLCPQYBSDMPJLPCJOQLCDHJJYSPRCHNWJNLHLYYQYYWZPTCZGWWMZFFJQQQQYXACLBHKDJXDGMMYDJXZLLSYGXGKJRYWZWYCLZMSSJZLDBYDCFCXYHLXCHYZJQSFQAGMNYXPFRKSSB" +
                                         "JLYXYSYGLNSCMHCWWMNZJJLXXHCHSYDSTTXRYCYXBYHCSMXJSZNPWGPXXTAYBGAJCXLYSDCCWZOCWKCCSBNHCPDYZNFCYYTYCKXKYBSQKKYTQQXFCWCHCYKELZQBSQYJQCCLMTHSYWHMKTLKJLYCXWHEQQHTQHZPQSQSCFYMMDMGBWHWLGSSLYSDLMLXPTHMJHWLJZYHZJXHTXJLHXRSWLWZJCBXMHZQXSDZPMGFCSGLSXYMJSHXPJXWMYQKSMYPLRTHBXFTPMHYXLCHLHLZYLXGSSSSTCLSLDCLRPBHZHXYYFHBBGDMYCNQQWLQHJJZYWJZYEJJDHPBLQXTQKWHLCHQXAGTLXLJXMSLXHTZKZJECXJCJNMFBYCSFYWYBJZGNYSDZSQYRSLJPCLPWXSDWEJBJCBCNAYTWGMPAPCLYQPCLZXSBNMSGGFNZJJBZSFZYNDXHPLQKZCZWALSBCCJXJYZGWKYP" +
                                         "SGXFZFCDKHJGXDLQFSGDSLQWZKXTMHSBGZMJZRGLYJBPMLMSXLZJQQHZYJCZYDJWBMJKLDDPMJEGXYHYLXHLQYQHKYCWCJMYYXNATJHYCCXZPCQLBZWWYTWBQCMLPMYRJCCCXFPZNZZLJPLXXYZTZLGDLDCKLYRZZGQTGJHHHJLJAXFGFJZSLCFDQZLCLGJDJCSNCLLJPJQDCCLCJXMYZFTSXGCGSBRZXJQQCTZHGYQTJQQLZXJYLYLBCYAMCSTYLPDJBYREGKLZYZHLYSZQLZNWCZCLLWJQJJJKDGJZOLBBZPPGLGHTGZXYGHZMYCNQSYCYHBHGXKAMTXYXNBSKYZZGJZLQJDFCJXDYGJQJJPMGWGJJJPKQSBGBMMCJSSCLPQPDXCDYYKYFCJDDYYGYWRHJRTGZNYQLDKLJSZZGZQZJGDYKSHPZMTLCPWNJAFYZDJCNMWESCYGLBTZCGMSSLLYXQSXSBSJS" +
                                         "BBSGGHFJLWPMZJNLYYWDQSHZXTYYWHMCYHYWDBXBTLMSYYYFSXJCSDXXLHJHFSSXZQHFZMZCZTQCXZXRTTDJHNNYZQQMNQDMMGYYDXMJGDHCDYZBFFALLZTDLTFXMXQZDNGWQDBDCZJDXBZGSQQDDJCMBKZFFXMKDMDSYYSZCMLJDSYNSPRSKMKMPCKLGDBQTFZSWTFGGLYPLLJZHGJJGYPZLTCSMCNBTJBQFKTHBYZGKPBBYMTTSSXTBNPDKLEYCJNYCDYKZDDHQHSDZSCTARLLTKZLGECLLKJLQJAQNBDKKGHPJTZQKSECSHALQFMMGJNLYJBBTMLYZXDCJPLDLPCQDHZYCBZSCZBZMSLJFLKRZJSNFRGJHXPDHYJYBZGDLQCSEZGXLBLGYXTWMABCHECMWYJYZLLJJYHLGBDJLSLYGKDZPZXJYYZLWCXSZFGWYYDLYHCLJSCMBJHBLYZLYCBLYDPDQYSXQZB" +
                                         "YTDKYXJYYCNRJMDJGKLCLJBCTBJDDBBLBLCZQRPXJCGLZCSHLTOLJNMDDDLNGKAQHQHJGYKHEZNMSHRPHQQJCHGMFPRXHJGDYCHGHLYRZQLCYQJNZSQTKQJYMSZSWLCFQQQXYFGGYPTQWLMCRNFKKFSYYLQBMQAMMMYXCTPSHCPTXXZZSMPHPSHMCLMLDQFYQXSZYJDJJZZHQPDSZGLSTJBCKBXYQZJSGPSXQZQZRQTBDKYXZKHHGFLBCSMDLDGDZDBLZYYCXNNCSYBZBFGLZZXSWMSCCMQNJQSBDQSJTXXMBLTXZCLZSHZCXRQJGJYLXZFJPHYMZQQYDFQJJLZZNZJCDGZYGCTXMZYSCTLKPHTXHTLBJXJLXSCDQXCBBTJFQZFSLTJBTKQBXXJJLJCHCZDBZJDCZJDCPRNPQCJPFCZLCLZXZDMXMPHJSGZGSZZQJYLWTJPFSYASMCJBTZKYCWMYTCSJJLJCQLWZM" +
                                         "ALBXYFBPNLSFHTGJWEJJXXGLLJSTGSHJQLZFKCGNNDSZFDEQFHBSAQTGLLBXMMYGSZLDYDQMJJRGBJTKGDHGKBLQKBDMBYLXWCXYTTYBKMRTJZXQJBHLMHMJJZMQASLDCYXYQDLQCAFYWYXQHZ";

            const string lsSecondCh = "ءآأؤإئابةتثجحخدذرزسشصضطظعغػؼؽ" +
                                        "ؾؿ������������������������������������������������������������������������������������������������������������������������������١٢٣٤٥٦٧٨٩٪٫٬٭ٮٯٰٱٲٳٴٵٶٷٸٹٺٻټٽپٿ������������������������������������������������������������������������������������������������������������������������������ڡڢڣڤڥڦڧڨکڪګڬڭڮگڰڱڲڳڴڵڶڷڸڹںڻڼڽھڿ����������������������������������������������������������������������������������" +
                                        "��������������������������������������������ۣۡۢۤۥۦۧۨ۩۪ۭ۫۬ۮۯ۰۱۲۳۴۵۶۷۸۹ۺۻۼ۽۾ۿ������������������������������������������������������������������������������������������������������������������������������ܡܢܣܤܥܦܧܨܩܪܫܬܭܮܯܱܴܷܸܹܻܼܾܰܲܳܵܶܺܽܿ������������������������������������������������������������������������������������������������������������������������������ݡݢݣݤݥݦݧݨݩݪݫݬݭݮݯݰݱݲݳݴݵݶ" +
                                        "ݷݸݹݺݻݼݽݾݿ������������������������������������������������������������������������������������������������������������������������������ޡޢޣޤޥަާިީުޫެޭޮޯްޱ޲޳޴޵޶޷޸޹޺޻޼޽޾޿������������������������������������������������������������������������������������������������������������������������������ߡߢߣߤߥߦߧߨߩߪ߲߫߬߭߮߯߰߱߳ߴߵ߶߷߸߹ߺ߻߼߽߾߿������������������������������������������������������������������������" +
                                        "�����������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������" +
                                        "����������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������" +
                                        "���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������" +
                                        "������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������" +
                                        "�����������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������" +
                                        "��������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������" +
                                        "���������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������" +
                                        "������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������" +
                                        "��������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������" +
                                        "������������������������������������������������������������������������������������������������������������������������������������������������������������������������������";

            string returnPy = "";
            foreach (var t in chineseCharacter)
            {
                var array = Encoding.Default.GetBytes(t.ToString(CultureInfo.InvariantCulture));
                //�Ǻ���
                if (array[0] < 176) { returnPy += t; }
                //һ������
                else if (array[0] >= 176 && array[0] <= 215)
                {

                    if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "z";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "ѹ", StringComparison.Ordinal) >= 0)
                        returnPy += "y";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "x";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "w";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "t";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "s";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "Ȼ", StringComparison.Ordinal) >= 0)
                        returnPy += "r";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "q";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "ž", StringComparison.Ordinal) >= 0)
                        returnPy += "p";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "Ŷ", StringComparison.Ordinal) >= 0)
                        returnPy += "o";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "n";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "m";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "l";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "k";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "j";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "h";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "g";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "f";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "e";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "d";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "c";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "b";
                    else if (String.Compare(t.ToString(CultureInfo.InvariantCulture), "��", StringComparison.Ordinal) >= 0)
                        returnPy += "a";
                }
                //��������
                else if (array[0] >= 215)
                {
                    returnPy += lsSecondEng.Substring(lsSecondCh.IndexOf(t.ToString(CultureInfo.InvariantCulture), 0, StringComparison.Ordinal), 1);
                }
            }
            return returnPy.ToUpper();
        }

        /// <summary>
        /// ȡ����ƴ��������ĸ
        /// </summary>
        /// <param name="chineseCharacter">����</param>
        /// <returns>����ĸ</returns>
        public static string GetFirstLetter2(string chineseCharacter)
        {
            var i = 0;
            var strResult = string.Empty;
            var unicode = Encoding.Unicode;
            var gbk = Encoding.GetEncoding(936);
            var unicodeBytes = unicode.GetBytes(chineseCharacter);
            var gbkBytes = Encoding.Convert(unicode, gbk, unicodeBytes);
            while (i < gbkBytes.Length)
            {
                if (gbkBytes[i] <= 127)
                {
                    strResult = strResult + (char)gbkBytes[i];
                    i++;
                }
                #region ���ɺ���ƴ������,ȡƴ������ĸ
                else
                {
                    var key = (ushort)(gbkBytes[i] * 256 + gbkBytes[i + 1]);
                    if (key >= '\uB0A1' && key <= '\uB0C4')
                    { strResult = strResult + "A"; }
                    else if (key >= '\uB0C5' && key <= '\uB2C0')
                    { strResult = strResult + "B"; }
                    else if (key >= '\uB2C1' && key <= '\uB4ED')
                    { strResult = strResult + "C"; }
                    else if (key >= '\uB4EE' && key <= '\uB6E9')
                    { strResult = strResult + "D"; }
                    else if (key >= '\uB6EA' && key <= '\uB7A1')
                    { strResult = strResult + "E"; }
                    else if (key >= '\uB7A2' && key <= '\uB8C0')
                    { strResult = strResult + "F"; }
                    else if (key >= '\uB8C1' && key <= '\uB9FD')
                    { strResult = strResult + "G"; }
                    else if (key >= '\uB9FE' && key <= '\uBBF6')
                    { strResult = strResult + "H"; }
                    else if (key >= '\uBBF7' && key <= '\uBFA5')
                    { strResult = strResult + "J"; }
                    else if (key >= '\uBFA6' && key <= '\uC0AB')
                    { strResult = strResult + "K"; }
                    else if (key >= '\uC0AC' && key <= '\uC2E7')
                    { strResult = strResult + "L"; }
                    else if (key >= '\uC2E8' && key <= '\uC4C2')
                    { strResult = strResult + "M"; }
                    else if (key >= '\uC4C3' && key <= '\uC5B5')
                    { strResult = strResult + "N"; }
                    else if (key >= '\uC5B6' && key <= '\uC5BD')
                    { strResult = strResult + "O"; }
                    else if (key >= '\uC5BE' && key <= '\uC6D9')
                    { strResult = strResult + "P"; }
                    else if (key >= '\uC6DA' && key <= '\uC8BA')
                    { strResult = strResult + "Q"; }
                    else if (key >= '\uC8BB' && key <= '\uC8F5')
                    { strResult = strResult + "R"; }
                    else if (key >= '\uC8F6' && key <= '\uCBF9')
                    { strResult = strResult + "S"; }
                    else if (key >= '\uCBFA' && key <= '\uCDD9')
                    { strResult = strResult + "T"; }
                    else if (key >= '\uCDDA' && key <= '\uCEF3')
                    { strResult = strResult + "W"; }
                    else if (key >= '\uCEF4' && key <= '\uD188')
                    { strResult = strResult + "X"; }
                    else if (key >= '\uD1B9' && key <= '\uD4D0')
                    { strResult = strResult + "Y"; }
                    else if (key >= '\uD4D1' && key <= '\uD7F9')
                    { strResult = strResult + "Z"; }
                    else { strResult = strResult + "?"; }
                    i = i + 2;
                }
                #endregion
            }
            return strResult;
        }
    }
}