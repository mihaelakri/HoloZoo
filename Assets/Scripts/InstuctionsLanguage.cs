using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class InstuctionsLanguage : MonoBehaviour
{
    // First page
    public Text instructionOneHeading; 
    public Text instructionOneMaterials;
    public Text instructionOneSupply; 
    // Second page
    public Text instructionTwo;
    // Third page
    public Text instructionThree; 
    public TMP_Text buttonFinish; 

    // Language
    private string selectedLanguage;

    // Translations dictionary
    private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>
    {
        {
            "en", new Dictionary<string, string>
            {
                { "instructionOneHeading", "Instructions for creating a projection hologram" },
                { "instructionOneMaterials", "Required Materials" },
                { "instructionOneSupply", "4 x A4 acetate paper\n1 x A4 paper\n1 x scissors\n1 x ruler\n1 x adhesive tape"},
                { "instructionTwo", "1. Measure screen diagonal.\n\n2. Draw an equilateral triangle with the side length exactly half of the screen diagonal.\n\n3. Divide two triangle sides into 3-10 of the length\n4. Cut drawn trapeze Tape the edges" },
                { "instructionThree", "Place the screen on the flat surface and put the pyramid on the centre of the screen" },
                { "buttonFinish", "Start" }
            }
        },
        {
            "fr", new Dictionary<string, string>
            {
                { "instructionOneHeading", "Instructions pour la création projecteur à hologramme" },
                { "instructionOneMaterials", "Matériel" },
                { "instructionOneSupply", "x4 feuilles d'acétate format A4\nx1 paire de ciseaux\nx1 règle\nx1 rouleau de ruban adhésif" },
                { "instructionTwo", "1. Mesurer la diagonale de l'écran\n2. Tracer un triangle équilatéral dont les côtés mesurent exactement la moitié de la diagonale de l'écran\n3. Diviser deux des côtés du triangle en  3/10 de leur longueur\n4. Découper le trapèze tracé\n5. Scotcher les bords" },
                { "instructionThree", "Placer l'écran sur une surface plane et positionner la pyramide au centre de l'écran" },
                { "buttonFinish", "Commencer" }
            }
        },
        {
            "hr", new Dictionary<string, string>
            {
                { "instructionOneHeading", "Upute za izradu projekcijskog holograma" },
                { "instructionOneMaterials", "Matériaux requis" },
                { "instructionOneSupply", "4 x A4 acetatni papir\n1 x A4 papir\n1 x škare\n1 x ravnalo\n1 x ljepljiva traka" },
                { "instructionTwo", "1. Izmjerite dijagonalu zaslona\n\n2. Nacrtajte jednakostranični trokut čija je stranica duljine točno polovice dijagonale zaslona\n\n3. Dvije stranice trokuta podijelite na 3/10 duljine" },
                { "instructionThree", "Postavite ekran na ravnu površinu i postavite piramidu na sredinu ekrana." },
                { "buttonFinish", "Započni" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                { "instructionOneHeading", "Instrucciones para crear un holograma de proyección" },
                { "instructionOneMaterials", "Materiales:" },
                { "instructionOneSupply", "4 x papel acetato tamaño A4\n1 x tijeras\n1 x regla\n1 x celo" },
                { "instructionTwo", "1. Mide la diagonal de la pantalla\n2. Dibuja un triángulo equilátero con la longitud del lado igual a la diagonal de la pantalla\n3. Divide dos lados del triángulo en 3/10 de la longitud\n4. Corta el trapecio dibujado\n5. Pega los borde con celo" },
                { "instructionThree", "Coloca la pantalla sobre la superficie plana y pon la pirámide en el centro de la pantalla" },
                { "buttonFinish", "Empieza" }
            }
        },
        {
            "hu", new Dictionary<string, string>
            {
                { "instructionOneHeading", "Utasítások a hologramos vetítéshez" },
                { "instructionOneMaterials", "Anyagok" },
                { "instructionOneSupply", "4 db A4-es átlátszó műanyag lap\n1 db olló\n1 db vonalzó\n1 db ragasztószalag" },
                { "instructionTwo", "1. Mérd meg a képernyő átlóját\n2. Rajzolj egy egyenlő oldalú háromszöget, amelynek oldalhossza pontosan a képernyő átlójának fele.\n3.  A háromszög két szomszédos oldalán jelöld meg a pontokat az oldal 3/10-ed részénél a sarok levágásához.\n4. Vágd ki a keletkezett trapézt\n5. Ragaszd össze a széleket" },
                { "instructionThree", "Tedd az eszközödet egy sík felületre, és helyezd a piramist a képernyő közepére." },
                { "buttonFinish", "Start" }
            }
        }
    };

    void Start()
    {
        selectedLanguage = PlayerPrefs.GetString("lang", "en");
        ApplyLanguageTexts();
    }

    void ApplyLanguageTexts()
    {
        if (translations.ContainsKey(selectedLanguage))
        {
            var languageTexts = translations[selectedLanguage];

            instructionOneHeading.text = languageTexts["instructionOneHeading"];
            instructionOneMaterials.text = languageTexts["instructionOneMaterials"];
            instructionOneSupply.text = languageTexts["instructionOneSupply"];
            instructionTwo.text = languageTexts["instructionTwo"];
            instructionThree.text = languageTexts["instructionThree"];
            buttonFinish.text = languageTexts["buttonFinish"];
        }
        else
        {
            Debug.LogWarning("Selected language not found: " + selectedLanguage);
        }
    }
}