using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

// Used to handle interactions with the UI
public class InterfaceHandler : MonoBehaviour
{
    // Makes InterfaceHandler accessible from other scripts
    private static InterfaceHandler instance;
    public static InterfaceHandler i { get { return instance; } }

    // Ref to UI that shows curr page amount
    [SerializeField] private TMP_Text pageText;

    // Ref to button that loads up a new game
    [SerializeField] private Button startButton;

    // Ref to audio sources used
    [SerializeField] private AudioSource song, loseSound, winSound;

    // Used to update end screen UI to show win or loss
    [SerializeField] private GameObject endScreenUI;
    [SerializeField] private RectTransform thumb;

    // Called before start
    void Awake()
    {
        // Sets static reference
        instance = gameObject.GetComponent<InterfaceHandler>();

        // Checks if song should play
        if(song != null)
        {
            // Play song
            song.Play();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Reset time scale since Once game is over, time scale is set to 0
        Time.timeScale = 1f;
    }

    // Called every frame to get user input
    void Update()
    {
        // Checks for user input and that the button is active
        if(Input.GetButtonDown("Input") && startButton.IsActive())
        {
            // Calls On Click event on the button as if it was clicked from mouse
            startButton.onClick.Invoke();
        }
    }

    // Loads a designated scene
    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadSceneAsync(sceneNum);
    }

    // Changes the UI for how many pages the player has collected
    public void UpdatePageText(int pageAmount)
    {
        pageText.text = pageAmount + "/5";
    }

    // Opens game over UI (both win and lose)
    public void OpenEndUI(bool didWin)
    {
        // Dynamically set UI so up = win and down = loss
        switch (didWin)
        {
            // Makes thumb appear up
            case true:
                thumb.localScale = new Vector3(-1, 1, 1);
                break;
            // Makes thumb appear down
            case false:
                thumb.localScale = new Vector3(-1, -1, 1);
                break;
        }

        // Opens game end UI after sprite is set
        endScreenUI.SetActive(true); 
    }

    // Plays sound for winning/losing a game
    public void PlayEndSound(bool didWin)
    {
        // Game is over so stop the looping song
        song.Stop();

        // Checks if the player won
        switch (didWin)
        {
            // Play win sound on win
            case true:
                winSound.Play();
                break;
            // Play lose sound on lose
            case false:
                loseSound.Play();
                break;
        }
    }
}
