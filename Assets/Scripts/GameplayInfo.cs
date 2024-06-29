using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using UnityEngine.UI;

public class GameplayInfo : AttributesSync
{
  //[SerializeField] public int hands = 4;
  //[SerializeField] public int discards = 3;
  [SerializeField] public int selectedHandCount = 0;
  [SerializeField] public int selectedMagicianCards = 0;
  [SerializeField] public int selectedJokerCount = 0;
  [SerializeField] public int selectedOwnedJokerCount = 0;
  [SerializeField] public int selectedOwnedTarotCount = 0;
  
  [SerializeField] public int currentRound = 1;
  [SerializeField] public int numberOfRounds = 4;
  [SynchronizableField] public int playerCount = 0;
  [SynchronizableField] public int currentPlayer = 0;
  [SynchronizableField] public string currentPlayerName;
  public int maxDiscards = 3;
  public int hands = 4;
   public int maxHands = 4;

  public int gameMaxPlayers = 4;
  public int TimeLimit = 30;
  public int gameRounds = 4;
  public int gameMaxJokers = 3;
  public int gameMoneyMult = 1;
  public int gameJokerChance = 75;
  public List<GameObject> selectedCards = new List<GameObject>();
  public GameObject selectedMagicianCardObj;
  public string selectedMagicianCardUsername;
  //public int myPlayerID;
  public string myName;

  public bool orderByRank = true;
  public bool initiatedOrdering = false;

  public List<string> ranks = new List<string>() {"A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2"};
  public List<string> suit = new List<string>() {"Treff", "Pikk", "Kőr", "Káró"};

  public Text CreateText(Transform parent, string textString, float offsetY)
  {
    var go = new GameObject();
    go.transform.parent = parent;
    var text = go.AddComponent<Text>();
    text.text = textString;
    text.transform.position = parent.transform.position - new Vector3(0, offsetY, 0);
    return text;
  }

  [NonSerialized] public List<string> handsNamesList = new List<string>{
    "FIVE OF A KIND",
    "ROYAL FLUSH",
    "STRAIGHT FLUSH",
    "FOUR OF A KIND",
    "FULL HOUSE",
    "FLUSH",
    "STRAIGHT",
    "THREE OF A KIND",
    "TWO PAIR",
    "PAIR",
    "HIGH CARD",
  };

  [NonSerialized] public Dictionary<string, int> handScores = new Dictionary<string,int>(){
    {"FIVE OF A KIND SCORE", 120},
    {"FIVE OF A KIND MULT", 12},
    {"ROYAL FLUSH SCORE", 100},
    {"ROYAL FLUSH MULT", 10},
    {"STRAIGHT FLUSH SCORE", 100},
    {"STRAIGHT FLUSH MULT", 8},
    {"FOUR OF A KIND SCORE", 60},
    {"FOUR OF A KIND MULT", 7},
    {"FULL HOUSE SCORE", 40},
    {"FULL HOUSE MULT", 4},
    {"FLUSH SCORE", 35},
    {"FLUSH MULT", 4},
    {"STRAIGHT SCORE", 30},
    {"STRAIGHT MULT", 4},
    {"THREE OF A KIND SCORE", 30},
    {"THREE OF A KIND MULT", 3},
    {"TWO PAIR SCORE", 20},
    {"TWO PAIR MULT", 2},
    {"PAIR SCORE", 10},
    {"PAIR MULT", 2},
    {"HIGH CARD SCORE", 5},
    {"HIGH CARD MULT", 1},
  };

  [NonSerialized] public Dictionary<string, string> handNamesHUN = new Dictionary<string,string>(){
    {"FIVE OF A KIND", "SZUPERPÓKER"},
    {"ROYAL FLUSH", "ROYAL FLUSH"},
    {"STRAIGHT FLUSH", "SZÍNSOR"},
    {"FOUR OF A KIND", "PÓKER"},
    {"FULL HOUSE", "FULL HOUSE"},
    {"FLUSH", "SZÍN"},
    {"STRAIGHT", "SOR"},
    {"THREE OF A KIND", "DRILL"},
    {"TWO PAIR", "KÉT PÁR"},
    {"PAIR", "PÁR"},
    {"HIGH CARD", "MAGAS LAP"},
  };

  [NonSerialized] public Dictionary<string, int> cardScore = new Dictionary<string,int>(){
    {"A", 11},
    {"K", 10},
    {"Q", 10},
    {"J", 10},
    {"10", 10},
    {"9", 9},
    {"8", 8},
    {"7", 7},
    {"6", 6},
    {"5", 5},
    {"4", 4},
    {"3", 3},
    {"2", 2}
  };

  [NonSerialized] public Dictionary<string, int> rankOrderDict = new Dictionary<string,int>(){
    {"A", 12},
    {"K", 11},
    {"Q", 10},
    {"J", 9},
    {"10", 8},
    {"9", 7},
    {"8", 6},
    {"7", 5},
    {"6", 4},
    {"5", 3},
    {"4", 2},
    {"3", 1},
    {"2", 0}
  };
  [NonSerialized] public Dictionary<string, int> suitOrderDict = new Dictionary<string,int>(){
    {"Káró", 3},
    {"Kőr", 2},
    {"Pikk", 1},
    {"Treff", 0}
  };

  [NonSerialized] public Dictionary<string, int> tarotCardsDict = new Dictionary<string,int>(){
    {"A BOLOND", 0},
    {"A MÁGUS", 1},
    {"A FŐPAPNŐ", 2},
    {"AZ URALKODÓNŐ", 3},
    {"AZ URALKODÓ", 4},
    {"A FŐPAP", 5},
    {"SZERETŐK", 6},
    {"DIADALSZEKÉR", 7},
    {"ERŐ", 8},
    {"REMETE", 9}
  };

  [NonSerialized] public Dictionary<string, int>countyCardsDict = new Dictionary<string, int>() {
    {"KOMÁROM-ESZTERGOM", 0}, 
    {"NÓGRÁD", 1}, 
    {"VAS", 2}, 
    {"HEVES", 3}, 
    {"TOLNA", 4}, 
    {"ZALA", 5}, 
    {"GYŐR-MOSON-SOPRON", 6},
    {"CSONGRÁD-CSANÁD", 7},
    {"FEJÉR", 8},
    {"BARANYA", 9}
  };

  [NonSerialized] public Dictionary<string, int> pokerCardsDict = new Dictionary<string,int>(){
    {"Treff A", 0},
    {"Treff 2", 1},
    {"Treff 3", 2},
    {"Treff 4", 3},
    {"Treff 5", 4},
    {"Treff 6", 5},
    {"Treff 7", 6},
    {"Treff 8", 7},
    {"Treff 9", 8},
    {"Treff 10", 9},
    {"Treff J", 10},
    {"Treff Q", 11},
    {"Treff K", 12},
    {"Pikk A", 13},
    {"Pikk 2", 14},
    {"Pikk 3", 15},
    {"Pikk 4", 16},
    {"Pikk 5", 17},
    {"Pikk 6", 18},
    {"Pikk 7", 19},
    {"Pikk 8", 20},
    {"Pikk 9", 21},
    {"Pikk 10", 22},
    {"Pikk J", 23},
    {"Pikk Q", 24},
    {"Pikk K", 25},
    {"Kőr A", 26},
    {"Kőr 2", 27},
    {"Kőr 3", 28},
    {"Kőr 4", 29},
    {"Kőr 5", 30},
    {"Kőr 6", 31},
    {"Kőr 7", 32},
    {"Kőr 8", 33},
    {"Kőr 9", 34},
    {"Kőr 10", 35},
    {"Kőr J", 36},
    {"Kőr Q", 37},
    {"Kőr K", 38},
    {"Káró A", 39},
    {"Káró 2", 40},
    {"Káró 3", 41},
    {"Káró 4", 42},
    {"Káró 5", 43},
    {"Káró 6", 44},
    {"Káró 7", 45},
    {"Káró 8", 46},
    {"Káró 9", 47},
    {"Káró 10", 48},
    {"Káró J", 49},
    {"Káró Q", 50},
    {"Káró K", 51}
  };

  [NonSerialized] public List<string> pokerCardImageNames = new List<string>() {
    "Treff A",
    "Treff 2",
    "Treff 3",
    "Treff 4",
    "Treff 5",
    "Treff 6",
    "Treff 7",
    "Treff 8",
    "Treff 9",
    "Treff 10",
    "Treff J",
    "Treff Q",
    "Treff K",
    "Pikk A",
    "Pikk 2",
    "Pikk 3",
    "Pikk 4",
    "Pikk 5",
    "Pikk 6",
    "Pikk 7",
    "Pikk 8",
    "Pikk 9",
    "Pikk 10",
    "Pikk J",
    "Pikk Q",
    "Pikk K",
    "Kőr A",
    "Kőr 2",
    "Kőr 3",
    "Kőr 4",
    "Kőr 5",
    "Kőr 6",
    "Kőr 7",
    "Kőr 8",
    "Kőr 9",
    "Kőr 10",
    "Kőr J",
    "Kőr Q",
    "Kőr K",
    "Káró A",
    "Káró 2",
    "Káró 3",
    "Káró 4",
    "Káró 5",
    "Káró 6",
    "Káró 7",
    "Káró 8",
    "Káró 9",
    "Káró 10",
    "Káró J",
    "Káró Q",
    "Káró K",
  };

  [NonSerialized] public Dictionary<string, int> _tarotImageNamesDict = new Dictionary<string, int>() {
    {"A BOLOND", 0},
    {"A MÁGUS", 1},
    {"A FŐPAPNŐ", 2},
    {"AZ URALKODÓNŐ", 3},
    {"AZ URALKODÓ", 4},
    {"A FŐPAP", 5},
    {"SZERETŐK", 6},
    {"DIADALSZEKÉR", 7},
    {"ERŐ", 8},
    {"REMETE", 9}
  };

  [NonSerialized] public Dictionary<string, int> _countyImageNamesDict = new Dictionary<string, int>() {
    {"KOMÁROM-ESZTERGOM", 0}, 
    {"NÓGRÁD", 1}, 
    {"VAS", 2}, 
    {"HEVES", 3}, 
    {"TOLNA", 4}, 
    {"ZALA", 5}, 
    {"GYŐR-MOSON-SOPRON", 6},
    {"CSONGRÁD-CSANÁD", 7},
    {"FEJÉR", 8},
    {"BARANYA", 9}
  };

  [NonSerialized] public Dictionary<string, int> _jokerImageNamesDict = new Dictionary<string, int>() {
    {"Piros ÁSZ", 3},
    {"Piros KIRÁLY", 2},
    {"Piros FELSŐ", 1},
    {"Piros ALSÓ", 0},
    {"Piros X", 4},
    {"Piros IX", 5},
    {"Piros VIII", 6},
    {"Piros VII", 7},
    {"Tök ÁSZ", 11},
    {"Tök KIRÁLY", 10},
    {"Tök FELSŐ", 9},
    {"Tök ALSÓ", 8},
    {"Tök X", 12},
    {"Tök IX", 13},
    {"Tök VIII", 14},
    {"Tök VII", 15},
    {"Zöld ÁSZ", 19},
    {"Zöld KIRÁLY", 18},
    {"Zöld FELSŐ", 17},
    {"Zöld ALSÓ", 16},
    {"Zöld X", 20},
    {"Zöld IX", 21},
    {"Zöld VIII", 22},
    {"Zöld VII", 23},
    {"Makk ÁSZ", 27},
    {"Makk KIRÁLY", 26},
    {"Makk FELSŐ", 25},
    {"Makk ALSÓ", 24},
    {"Makk X", 28},
    {"Makk IX", 29},
    {"Makk VIII", 30},
    {"Makk VII", 31}
  };
  [NonSerialized] public Dictionary<string, int> _jokerCardPricesComplex = new Dictionary<string, int>() {
    {"Piros ÁSZ", 8},
    {"Piros KIRÁLY", 7},
    {"Piros FELSŐ", 6},
    {"Piros ALSÓ", 5},
    {"Piros X", 4},
    {"Piros IX", 3},
    {"Piros VIII", 2},
    {"Piros VII", 1},
    {"Tök ÁSZ", 8},
    {"Tök KIRÁLY", 7},
    {"Tök FELSŐ", 6},
    {"Tök ALSÓ", 5},
    {"Tök X", 4},
    {"Tök IX", 3},
    {"Tök VIII", 2},
    {"Tök VII", 1},
    {"Zöld ÁSZ", 8},
    {"Zöld KIRÁLY", 7},
    {"Zöld FELSŐ", 6},
    {"Zöld ALSÓ", 5},
    {"Zöld X", 4},
    {"Zöld IX", 3},
    {"Zöld VIII", 2},
    {"Zöld VII", 1},
    {"Makk ÁSZ", 8},
    {"Makk KIRÁLY", 7},
    {"Makk FELSŐ", 6},
    {"Makk ALSÓ", 5},
    {"Makk X", 4},
    {"Makk IX", 3},
    {"Makk VIII", 2},
    {"Makk VII", 1}
  };

  [NonSerialized] public Dictionary<string, int> _jokerCardPricesSimple = new Dictionary<string, int>() {
    {"ÁSZ", 8},
    {"KIRÁLY", 7},
    {"FELSŐ", 6},
    {"ALSÓ", 5},
    {"X", 4},
    {"IX", 3},
    {"VIII", 2},
    {"VII", 1},
  };

  [NonSerialized] public Dictionary<string, int> _tarotCardPricesSimple = new Dictionary<string, int>() {
    {"A BOLOND", 1},
    {"A MÁGUS", 2},
    {"A FŐPAPNŐ", 4},
    {"AZ URALKODÓNŐ", 3},
    {"AZ URALKODÓ", 4},
    {"A FŐPAP", 3},
    {"SZERETŐK", 5},
    {"DIADALSZEKÉR", 5},
    {"ERŐ", 6},
    {"REMETE", 12},
  };

  [NonSerialized] public Dictionary<string, int> _countyCardPricesSimple = new Dictionary<string, int>() {
    {"KOMÁROM-ESZTERGOM", 3}, 
    {"NÓGRÁD", 3}, 
    {"VAS", 3}, 
    {"HEVES", 3}, 
    {"TOLNA", 3}, 
    {"ZALA", 3}, 
    {"GYŐR-MOSON-SOPRON", 3},
    {"CSONGRÁD-CSANÁD", 3},
    {"FEJÉR", 3},
    {"BARANYA", 3}
  };

  [NonSerialized] public Dictionary<string, string> _jokerCardDescriptionSimple = new Dictionary<string, string>() {
    {"ÁSZ", "+5 <color=red>szorzó</color=red>"},
    {"KIRÁLY", "+3 <color=red>szorzó</color=red>"},
    {"FELSŐ", "+1 <color=blue>hand</color>"},
    {"ALSÓ", "+1 <color=red>discard</color>"},
    {"X", "+12 <color=blue>pont</color=blue> kártyánként 5 lapos hand esetén"},
    {"IX", "Minden kijátszott páratlan lap +2 <color=red>szorzó</color=red> (<color=purple>A</color>, <color=purple>9</color>, <color=purple>7</color>, <color=purple>5</color>, <color=purple>3</color>)"},
    {"VIII", "Minden kijátszott páros lap +20 <color=blue>pont</color>\n(<color=purple>10</color>, <color=purple>8</color>, <color=purple>6</color>, <color=purple>4</color>, <color=purple>2</color>)"},
    {"VII", "+2 <color=red>szorzó</color=red> ha nem volt discard használva"},
    {"Piros", "+1 hand size (MAX +1)"},
    {"Zöld", "+ 100 X round <color=blue>pont</color=blue> / round"},
    {"Makk", "+5 <color=blue>pont</color=blue> minden kártyához"},
    {"Tök", "+2<color=yellow>€</color> / round"},
  };

  [NonSerialized] public Dictionary<string, string> _tarotCardDescriptionSimple = new Dictionary<string, string>() {
    {"A BOLOND", "Maximum 2 kiválasztott kártyát <color=red>elpusztít</color> véglegesen"},
    {"A MÁGUS", "<color=red>Ellophatsz</color> egy kártyát véglegesen egy játékostól, azonnal kézbe kerül"},
    {"A FŐPAPNŐ", "Minden játékosnak megfordítja a kézben tartott kártyáit"},
    {"AZ URALKODÓNŐ", "1 kiválasztott kártyából csinál két klónt a pakliba, azonnal kézbe adja őket"},
    {"AZ URALKODÓ", "+25 <color=blue>pontot</color> ad maximum 2 kiválasztott kártyához"},
    {"A FŐPAP", ""},
    {"SZERETŐK", "+4 <color=red>szorzót</color> ad max. 2 kiválasztott kártyához"},
    {"DIADALSZEKÉR", "Megduplázza a jelenlegi pénzt (Max. 20€)\n<color=red>AZONNALI HATÁS</color>"},
    {"ERŐ", "<b>Negatívvá alakít</b> max. 2 kártyát (figyelmen kívül hagyják a <color=blue>hand size</color>-ot)"},
    {"REMETE", "+1 <b>joker</b> hely (max. 10)\n<color=red>AZONNALI HATÁS</color>"},
  };
  [NonSerialized] public Dictionary<string, string> _countyCardDescriptionSimple = new Dictionary<string, string>() {
    {"KOMÁROM-ESZTERGOM", "MAGAS LAP FEJLESZTÉS"}, 
    {"NÓGRÁD", "PÁR FEJLESZTÉS"}, 
    {"VAS", "KÉT PÁR FEJLESZTÉS"}, 
    {"HEVES", "DRILL FEJLESZTÉS"}, 
    {"TOLNA", "SOR FEJLESZTÉS"}, 
    {"ZALA", "SZÍN FEJLESZTÉS"}, 
    {"GYŐR-MOSON-SOPRON", "FULL HOUSE FEJLESZTÉS"},
    {"CSONGRÁD-CSANÁD", "PÓKER FEJLESZTÉS"},
    {"FEJÉR", "SZÍNSOR FEJLESZTÉS"},
    {"BARANYA", "SZUPERPÓKER FEJLESZTÉS"}
  };

  [NonSerialized] public Dictionary<string, string> _countyCardNameToHandNameDict = new Dictionary<string, string>() {
    {"KOMÁROM-ESZTERGOM", "HIGH CARD"}, 
    {"NÓGRÁD", "PAIR"}, 
    {"VAS", "TWO PAIR"}, 
    {"HEVES", "THREE OF A KIND"}, 
    {"TOLNA", "STRAIGHT"}, 
    {"ZALA", "FLUSH"}, 
    {"GYŐR-MOSON-SOPRON", "FULL HOUSE"},
    {"CSONGRÁD-CSANÁD", "FOUR OF A KIND"},
    {"FEJÉR", "STRAIGHT FLUSH"},
    {"BARANYA", "FIVE OF A KIND"}
  };

  [NonSerialized] public Dictionary<string, int> _countyCardScoreMultDict = new Dictionary<string,int>(){
    {"BARANYA SCORE", 40},
    {"BARANYA MULT", 5},
    {"FEJÉR SCORE", 40},
    {"FEJÉR MULT", 4},
    {"CSONGRÁD-CSANÁD SCORE", 40},
    {"CSONGRÁD-CSANÁD MULT", 3},
    {"GYŐR-MOSON-SOPRON SCORE", 30},
    {"GYŐR-MOSON-SOPRON MULT", 3},
    {"ZALA SCORE", 30},
    {"ZALA MULT", 2},
    {"TOLNA SCORE", 25},
    {"TOLNA MULT", 2},
    {"HEVES SCORE", 20},
    {"HEVES MULT", 2},
    {"VAS SCORE", 15},
    {"VAS MULT", 2},
    {"NÓGRÁD SCORE", 15},
    {"NÓGRÁD MULT", 1},
    {"KOMÁROM-ESZTERGOM SCORE", 10},
    {"KOMÁROM-ESZTERGOM MULT", 1},
  };

  [NonSerialized] public Dictionary<string, Dictionary<string, string>> tarotDeckCardNamesDescDict = new Dictionary<string, Dictionary<string, string>>() {
    {"mystical", new Dictionary<string, string>{
        {"A BOLOND", "A;"}, 
        {"A MÁGUS", "B;"}, 
        {"A FŐPAPNŐ", "C;"}, 
        {"AZ URALKODÓNŐ", "D;"}, 
        {"AZ URALKODÓ", "E;"}, 
        {"A FŐPAP", "F;"}, 
        {"SZERETŐK", "G;"}, 
        {"DIADALSZEKÉR", "H;"}, 
        {"ERŐ", "I;"}, 
        {"REMETE", "J;"}, 
      }
    },
  };
  [NonSerialized] public Dictionary<string, Dictionary<string, string>> countyDeckCardNamesDescDict = new Dictionary<string, Dictionary<string, string>>() {
    {"varmegye", new Dictionary<string, string>{
        {"KOMÁROM-ESZTERGOM", "A;"}, 
        {"NÓGRÁD", "B;"}, 
        {"VAS", "C;"}, 
        {"HEVES", "D;"}, 
        {"TOLNA", "E;"}, 
        {"ZALA", "F;"}, 
        {"GYŐR-MOSON-SOPRON", "FULL HOUSE;"},
        {"CSONGRÁD-CSANÁD", "FOUR OF A KIND;"},
        {"FEJÉR", "STRAIGHT FLUSH;"},
        {"BARANYA", "FIVE OF A KIND;"}
      }
    },
  };

  [NonSerialized] public Dictionary<string, Dictionary<string, string>> jokerDeckCardNamesDescDict = new Dictionary<string, Dictionary<string, string>>() {
    {"hun_historical_figures", new Dictionary<string, string>{
        {"Piros ÁSZ", "Zrínyi Ilona;(1643-1703)\nPéter és Frangepán Katalin leánya. II. Rákóczi Ferenc anyja."}, 
        {"Piros KIRÁLY", "II. Rákóczi Ferenc;(1676-1735)\n Magyar főnemes, a Rákóczi-szabadságharc vezetője, erdélyi fejedelem, birodalmi herceg."}, 
        {"Piros FELSŐ", "Bercsényi Miklós;(1665-1725)\nKuruc főgenerális, II. Rákóczi Ferenc közeli harcostársa, a Rákóczi-szabadságharc egyik irányítója."}, 
        {"Piros ALSÓ", "Dobó István;(1502-1572)\nMagyar katona, aki leginkább Eger várkapitányaként ismeretes."}, 
        {"Piros X", "Magas Tátra, Felvidék;A Kárpátok legmagasabb hegyvonulata, s egyben a világ legkisebb magashegysége a Tátra keleti részén, Szlovákia és Lengyelország határán."}, 
        {"Piros IX", "Sárospatak vára;A magyarországi késő reneszánsz építészet legértékesebb alkotása, Sárospatak legjelentősebb műemléke. A magyar nemzeti örökség része."}, 
        {"Piros VIII", "Rákóczi pénze;(1704) II. Rákóczi Ferenc leiratban tudatta a vármegyékkel és a városokkal, hogy az ezüstpénz hiánya miatt váltópénzül rézpénzt bocsát ki. Nehogy súlya miatt használhatatlan legyen, értéke szerint kettőt veretett: a polturát és a libertast."}, 
        {"Piros VII", "Kuruc vitéz;A 17-18. századi Magyarország területén vívott, Habsburg-ellenes felkelésekben részt vevő katona, illetve a velük rokonszenvező."}, 
        {"Tök ÁSZ", "Szilágyi Erzsébet;(1410-1484)\nHunyadi János magyar kormányzó felesége, Hunyadi László horvát bán és I. Mátyás magyar király anyja, Szilágyi Mihály kormányzó testvére."}, 
        {"Tök KIRÁLY", "Hunyadi Mátyás;(1443-1490)\n Magyarország és Horvátország királya 1458-tól, cseh király 1469-től, Ausztria uralkodó főhercege 1486-tól haláláig. A közkeletű Corvin Mátyás vagy Igazságos Mátyás néven is ismert."}, 
        {"Tök FELSŐ", "Kinizsi Pál;(1431-1494)\nA Magyar Királyság országbírója, Zala vármegye ispánja; a magyar történelem egyik legismertebb hadvezére."}, 
        {"Tök ALSÓ", "Dózsa György;(1470-1514)\nRégi székely nemesi család, a háromszéki dálnoki lófőcsalád sarja, végvári vitéz, az 1514. évi magyar parasztfelkelés vezetője."}, 
        {"Tök X", "Galambóc vára, Délvidék;Középkori erődítmény a Duna déli oldalán, a szerbiai Galambóc városától 4 kilométerre keletre, amelynek feladata a Vaskapu-szoros bejáratának ellenőrzése volt."}, 
        {"Tök IX", "Vajdahunyad vára;Alpár Ignác építész alkotása Budapest XIV. kerületében, a Városligetben. A Városligeti-tó Széchenyi-szigetén található, mely négy hídon keresztül érhető el."}, 
        {"Tök VIII", "Lajos király kettős keresztje;Fasz tudja"}, 
        {"Tök VII", "Fekete sereg katonája;A fekete sereg Mátyás király állandó zsoldoshadseregének a király halála után kialakult elnevezése."},
        {"Zöld ÁSZ", "Szent Erzsébet;(1207-1231)\nII. András magyar király és Merániai Gertrúd lánya."}, 
        {"Zöld KIRÁLY", "Szent László; (1040-1095)\n1077-től magyar király, 1091-től haláláig pedig horvát király. I. Béla király és Richeza királyné másodszülött fia. Nevéhez fűződik a magántulajdon védelmének megszilárdítása, Horvátország elfoglalása és az első magyar szentek avatása."}, 
        {"Zöld FELSŐ", "Koppány vezér;(962/964-997)\nGéza magyar fejedelem rokonának, Tar Zerindnek a fia volt. A jelenleg legelfogadottabb tudományos álláspont szerint Géza halála után a sztyeppi népekre jellemző szeniorátus elve alapján magának követelte a hatalmat és feleségül akarta venni Sarolt fejedelemasszonyt, Géza feleségét."}, 
        {"Zöld ALSÓ", "Julianus barát;(13. század)\nDomonkos-rendi szerzetes volt, aki Gerhardusszal és még néhány szerzetestársával két utazást tett, hogy megkeresse a magyarok őshazáját keleten."}, 
        {"Zöld X", "Tordai hasadék, Erdély;Mészkő-hasadék a Torockói-hegységben, Erdélyben, nem messze Torda városától. 1938 óta védett terület."}, 
        {"Zöld IX", "a régi Budai vár;Budapest I. kerületének egyik városrésze Vár néven, Buda városának ősi területe. 1987 óta az UNESCO világörökség listáján Budai Várnegyed néven szerepel."}, 
        {"Zöld VIII", "Szent Korona;Európa egyik legrégebben használt és mai napig épségben megmaradt beavató koronája. A magyar államiság egyik jelképe, mely végigkísérte a magyar történelmet legalább a 12. századtól napjainkig."}, 
        {"Zöld VII", "Árpád-házi lovag;Az Árpád-kori magyar hadsereg elsősorban lovas hadsereg volt. A nehézlovasság aránylag kis létszámú volt, a sereg tömegét az úgynevezett közepes lovasság adta. Ezt egészítették ki a segédnépek könnyűlovas csapatai."}, 
        {"Makk ÁSZ", "Emese fejedelemasszony;Az ősmagyar hitvilág szerint az Árpád-ház ősanyja. Férje valószínűleg Ügyek törzsfő, fia Álmos vezér volt."}, 
        {"Makk KIRÁLY", "Attila király;(410-453)\nAz európai hunok leghíresebb királya. Attila kora egyik leghatalmasabb nomád birodalmát uralta 434-től, befolyása Közép-Európától a Kaszpi-tengerig és a Dunától a Balti-tengerig terjedt."}, 
        {"Makk FELSŐ", "Árpád vezér;(845-907)\nA magyar törzsszövetség nagyfejedelme volt a honfoglalás idején - melynek során a magyarok Etelközből a Kárpát-medencébe települtek. Tőle származnak az Árpád-ház uralkodói, akik halálát követően négy évszázadon át uralkodtak Magyarországon."}, 
        {"Makk ALSÓ", "Botond vitéz;(10. század)\nA kalandozások korának egyik legendás alakja."}, 
        {"Makk X", "Vereckei hágó, Kárpátalja;Az Északkeleti-Kárpátok gerincén, a Latorca és a Sztrijbe ömlő Opir folyó völgye között helyezkedik el. A hágó tengerszint feletti magassága 841 méter."}, 
        {"Makk IX", "Attila király fapalotája;Geci tudja"}, 
        {"Makk VIII", "Attila király kardja;Attila kardja, Mars kardja, avagy Isten kardja, Attila hun király legendás fegyvere volt. Jordanes, a történész Priszkosz munkáját idézve meséli el a kard történetét."}, 
        {"Makk VII", "Szittya harcos;A szittya harcos a gyengék és elesettek oltalmazója."}, 
      }
    },
  };

  [NonSerialized]public List<string> _tarotCardImageNamesList = new List<string>() {
    //"A BOLOND", "A MÁGUS", "A FŐPAPNŐ", "AZ URALKODÓNŐ", "AZ URALKODÓ", "A FŐPAP", "SZERETŐK", "DIADALSZEKÉR", "ERŐ", "REMETE"
    "A BOLOND", "A MÁGUS", "A FŐPAPNŐ", "AZ URALKODÓNŐ", "AZ URALKODÓ", "SZERETŐK", "DIADALSZEKÉR", "ERŐ", "REMETE"
  };
  [NonSerialized]public List<string> _countyCardImageNamesList = new List<string>() {
    "KOMÁROM-ESZTERGOM", "NÓGRÁD", "VAS", "HEVES", "TOLNA", "ZALA", "GYŐR-MOSON-SOPRON", "CSONGRÁD-CSANÁD", "FEJÉR", "BARANYA"
  };

  [NonSerialized] public List<string> _jokerCardImageNamesList = new List<string>() {
    "Piros ÁSZ",
    "Piros KIRÁLY",
    "Piros FELSŐ",
    "Piros ALSÓ",
    "Piros X",
    "Piros IX",
    "Piros VIII",
    "Piros VII",
    "Tök ÁSZ",
    "Tök KIRÁLY",
    "Tök FELSŐ",
    "Tök ALSÓ",
    "Tök X",
    "Tök IX",
    "Tök VIII",
    "Tök VII",
    "Zöld ÁSZ",
    "Zöld KIRÁLY",
    "Zöld FELSŐ",
    "Zöld ALSÓ",
    "Zöld X",
    "Zöld IX",
    "Zöld VIII",
    "Zöld VII",
    "Makk ÁSZ",
    "Makk KIRÁLY",
    "Makk FELSŐ",
    "Makk ALSÓ",
    "Makk X",
    "Makk IX",
    "Makk VIII",
    "Makk VII"
  };

  void Start()
  {
    //selectedCards.Clear();
    //selectedHandCount = 0;
  }
}
