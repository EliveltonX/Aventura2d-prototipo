using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class MenusController : MonoBehaviour
{
    private Controles controles;

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject inventoryScreen;
    public GameObject dieMenu;
    [Space(2)]
    [Header("Buttons")]
    public GameObject pauseFirstbtn;
    public GameObject optionsFirstBtn;
    public GameObject inventoryFirstBtn;
    public GameObject dieFirstBtn;
    [Space(2)]
    [Header("Others")]
    public PlayerController charController;

    private void Awake()
    {
        controles = new Controles();
        controles.Gameplay.Start.started += ctx => PauseShower();
        controles.Gameplay.Inventory.started += ctx => InventoryShower();
        Time.timeScale = 1;


    }


    void InventoryShower()
    {

        
        if (inventoryScreen.activeInHierarchy)
        {
            inventoryScreen.SetActive(false);
            SelectNewEventObject(null);
            
        }
        else if (!pauseMenu.activeInHierarchy)
        {
            inventoryScreen.SetActive(true);
            SelectNewEventObject(inventoryFirstBtn);
            

        }
        PauseGameMechanics();
    }
    void PauseShower() 
    {

        //Debug.Log("Start has Pressed");
        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
            SelectNewEventObject(null);
            
        }
        else 
        {
            pauseMenu.SetActive(true);
            SelectNewEventObject(pauseFirstbtn);
            

        }
        PauseGameMechanics();
    }


    public void PauseGameMechanics() 
    {
        if (!pauseMenu.activeInHierarchy && !inventoryScreen.activeInHierarchy && !optionsMenu.activeInHierarchy && !dieMenu.activeInHierarchy)
        {
            Time.timeScale = 1;
            charController.enabled = true;
        }
        else 
        {
            Time.timeScale = 0;
            charController.enabled = false;
        }
        
    }

    public void DieMenuShower() 
    {
        dieMenu.SetActive(true);
        PauseGameMechanics();
        SelectNewEventObject(dieFirstBtn);
       
        
    }
    public void loadScene() 
    {
        Time.timeScale = 1;
        dieMenu.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void SelectNewEventObject(GameObject _obj) 
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_obj);
    }
    private void OnEnable()
    {
        controles.Gameplay.Enable();
    }
    private void OnDisable()
    {
        controles.Gameplay.Disable();
    }
}
