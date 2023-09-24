import android.content.Context;
import android.speech.tts.TextToSpeech;
import android.speech.tts.TextToSpeech.OnInitListener;
import android.util.Log;

public class TextToSpeechPlugin implements OnInitListener {

    private TextToSpeech textToSpeech;
    private String textToSpeak;

    public TextToSpeechPlugin(Context context) {
        textToSpeech = new TextToSpeech(context, this);
    }

    public void speak(String text) {
        textToSpeak = text;
        textToSpeech.speak(text, TextToSpeech.QUEUE_FLUSH, null, null);
    }

    @Override
    public void onInit(int status) {
        if (status == TextToSpeech.SUCCESS) {
            int result = textToSpeech.setLanguage(java.util.Locale.US);

            if (result == TextToSpeech.LANG_MISSING_DATA ||
                result == TextToSpeech.LANG_NOT_SUPPORTED) {
                Log.e("TextToSpeechPlugin", "Language not supported.");
            }
        } else {
            Log.e("TextToSpeechPlugin", "Initialization failed.");
        }
    }
}