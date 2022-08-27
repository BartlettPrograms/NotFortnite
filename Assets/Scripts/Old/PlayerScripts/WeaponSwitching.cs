using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] private GameObject weaponContainer;
    
    // Importing player input again and again. Make a pInputclass
    private PlayerInput playerInput;

    private int previousSelectedWeapon;
    public int selectedWeapon = 0;
    
    // Gunscript references
    private GunSystemv2[] GunScriptsArray;


    private void Awake()
    {
        // Instantiate array and size for gun scripts
        GunScriptsArray = new GunSystemv2[weaponContainer.transform.childCount];
        // Import Controls
        NewControlScheme playerInputActions = new NewControlScheme();
        playerInputActions.Player.Enable();
        playerInputActions.Player.SelectWeapon1.performed += selectWeapon1;
        playerInputActions.Player.SelectWeapon2.performed += selectWeapon2;
        playerInputActions.Player.SelectWeapon3.performed += selectWeapon3;
        playerInputActions.Player.SelectWeapon4.performed += selectWeapon4;
        
        // Get gunScripts from all weapons
        int x = 0;
        foreach (Transform weapon in transform)
        {
            GunScriptsArray[x] = weapon.GetComponent<GunSystemv2>();
            x++;
        }
        
    }

    void Start()
    {
        //Select first weapon when starting game
        selectWeapon();
    }

    
    // This Function changes the held weapon to the selected weapon
    private void selectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                //GunScriptsArray[i].allowToShoot(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
                //GunScriptsArray[i].allowToShoot(false);
            }
            i++;
        }
    }
    
    
    // Logic to execute when pressing change weapon buttons
    // Surely this can be less ugly. who knows
    private void selectWeapon1(InputAction.CallbackContext context)
    {
        previousSelectedWeapon = selectedWeapon;
        // change selected weapon variable
        if (context.performed && transform.childCount >= 1)
        {
            selectedWeapon = 0;
        }
        // if weapons change, call change weapon method
        checkWeapon(previousSelectedWeapon);
    }
    private void selectWeapon2(InputAction.CallbackContext context)
    {
        previousSelectedWeapon = selectedWeapon;
        // change selected weapon variable
        if (context.performed && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        // if weapons change, call change weapon method
        checkWeapon(previousSelectedWeapon);
    }
    private void selectWeapon3(InputAction.CallbackContext context)
    {
        previousSelectedWeapon = selectedWeapon;
        // change selected weapon variable
        if (context.performed && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        // if weapons change, call change weapon method
        checkWeapon(previousSelectedWeapon);
    }
    private void selectWeapon4(InputAction.CallbackContext context)
    {
        previousSelectedWeapon = selectedWeapon;
        // change selected weapon variable
        if (context.performed && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }
        // if weapons change, call change weapon method
        checkWeapon(previousSelectedWeapon);
    }

    
    // Extra function checks if the weapon needs to be changed, then calls selectWeapon()
    private void checkWeapon(int previousWeapon)
    {
        if (previousWeapon != selectedWeapon)
        {
            selectWeapon();
        }
    }
}
