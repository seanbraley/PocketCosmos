using UnityEngine;
using UnityEngine.UI;
using Superbest_random; // Included for gaussian
using System.Collections;
using Completed;

public class StarMenu : MonoBehaviour
{
    public static StarMenu Instance;

    private Text _nameText;  // Later

    private Button _shipsButton;
    private Button _viewSystemButton;

    private Star star;


	public void Awake() {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Bye Bye instance");
            Destroy(Instance.gameObject);
        }
        Instance = this; //shitty singleton, but anything more ruins everything for whatever reason.
                         // I'm not even mad rn.


        _nameText = transform.Find("Name").GetComponent<Text>();

        _shipsButton = transform.Find("ShipsButton").GetComponent<Button>();
        _shipsButton.onClick.AddListener(() => ShowShipMenu());

	    _viewSystemButton = transform.Find("ViewSystemButton").GetComponent<Button>();
        _viewSystemButton.onClick.AddListener(() => EnterSystem());

        gameObject.SetActive(false);
    }

    public void ShowShipMenu()
    {
        Debug.Log("ShowShipMenu");
        ShipSelectMenu.Instance.PopulateShipSelectMenu(star.myNumber);
        ShipSelectMenu.Instance.gameObject.SetActive(true);
        ShipSelectMenu.Instance.transform.SetAsLastSibling();
        transform.SetAsFirstSibling();
    }

    public void EnterSystem()
    {
        Debug.Log("EnterSystem");
        GameManager.instance.ToSystemView();
    }

    public void SetInfo(Star star)
    {
        this.star = star;
    }
}
