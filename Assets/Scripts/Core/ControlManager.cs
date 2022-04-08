using Buildings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
    
namespace Core
{
    public class ControlManager : MonoBehaviour
    {
        [SerializeField] private LayerMask buildingsMask;

        public bool CanBuild;
        
        private InputActions controls;
        private InputAction input_select;
        private InputAction input_disselect;
        private InputAction input_sell;
        private InputAction input_upgrade;
        private InputAction input_populate;
        private InputAction input_depopulate;
        private InputAction input_launch;
        
        [SerializeField] private Building _selectedBuilding;
        [SerializeField] private Building _preparedBuilding;
        [SerializeField] private GameObject _sprite;
        [SerializeField] private GameObject _preparedBuildingSprite;
        [SerializeField] private bool _isBuilding;
        private Vector3 _mousePos;

        public static ControlManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            
            controls = new InputActions();
        }

        private void OnEnable()
        {
            input_select = controls.Controls.Select;
            input_disselect = controls.Controls.Disselect;
            input_sell = controls.Controls.Sell;
            input_populate = controls.Controls.Populate;
            input_depopulate = controls.Controls.Depopulate;
            input_upgrade = controls.Controls.Upgrade;
            input_launch = controls.Controls.LaunchRocket;
            
            input_select.Enable();
            input_disselect.Enable();
            input_populate.Enable();
            input_depopulate.Enable();
            input_sell.Enable();
            input_upgrade.Enable();
            input_launch.Enable();
            
            input_select.performed += Select;
            input_disselect.performed += Disselect;
            input_populate.performed += Populate;
            input_depopulate.performed += Depopulate;
            input_upgrade.performed += Upgrade;
            input_sell.performed += Sell;
            input_launch.performed += LaunchRocket;
        }

        private void Populate(InputAction.CallbackContext context)
        {
            if(_selectedBuilding != null)
                _selectedBuilding.Populate();
        }
        
        private void Depopulate(InputAction.CallbackContext context)
        {
            if(_selectedBuilding != null)
                _selectedBuilding.Depopulate();
        }

        private void Sell(InputAction.CallbackContext context)
        {
            if(_selectedBuilding != null)
                _selectedBuilding.Sell();
        }
        
        private void Upgrade(InputAction.CallbackContext context)
        {
            if(_selectedBuilding != null)
                _selectedBuilding.Upgrade();
        }
        
        private void LaunchRocket(InputAction.CallbackContext context)
        {
            if (_selectedBuilding != null)
            {
                Rocket rocket = _selectedBuilding.GetComponent<Rocket>();
                    if(rocket != null)
                        rocket.LaunchRocket();
            }
        }

        private void OnDisable()
        {
            input_select.Disable();
        }

        private void Update()
        {
            _mousePos = Mouse.current.position.ReadValue();
            
            if (_isBuilding && _preparedBuildingSprite != null)
            {
                // Debug.Log("Placing the building");
                var t = Camera.main.ScreenToWorldPoint(_mousePos);
                _preparedBuildingSprite.transform.position = (Vector2) t;
            }
        }

        private void Select(InputAction.CallbackContext context)
        {
            if (_isBuilding)
            {
                if(CanBuild) Build();
            }
            else
            {
                if(EventSystem.current.IsPointerOverGameObject())
                    return;
                
                Ray ray = Camera.main.ScreenPointToRay(_mousePos);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, buildingsMask);
                
                if (hit.collider != null)
                {
                    Building building = hit.collider.GetComponentInParent<Building>();

                    if (building != null)
                    {
                        if (building != _selectedBuilding)
                        {
                            if(_selectedBuilding != null)
                                _selectedBuilding.OnDisselected();
                            
                            _selectedBuilding = building;
                            _selectedBuilding.OnSelected();
                        }
                    }
                    
                    // Debug.Log("Building Selected");
                }
                else
                {
                    Disselect(context);
                }
            }
        }

        private void Disselect(InputAction.CallbackContext context)
        {
            if (_selectedBuilding != null)
            {
                _selectedBuilding.OnDisselected();
                _selectedBuilding = null;
            }

            if (_isBuilding)
            {
                Destroy(_preparedBuildingSprite);
                _preparedBuilding = null;
                _isBuilding = false;
                CanBuild = false;
            }
            
            // Debug.Log("Selection Canceled");
        }

        public void PrepareSprite(GameObject sprite)
        {
            // Debug.Log("Sprite prepared");
            _sprite = sprite;
        }

        public void PrepareToBuild(Building building)
        {
            Disselect(new InputAction.CallbackContext());
            _isBuilding = true;
            CanBuild = true;
            _preparedBuilding = building;
            _preparedBuildingSprite = Instantiate(_sprite, (Vector2) Camera.main.ScreenToWorldPoint(_mousePos), Quaternion.identity);
            // Debug.Log("Building prepared to be built");
        }

        private void Build()
        {
            if ((_preparedBuilding.GetCost() > GameManager.Instance.Gold) ||
                (GameManager.Instance.Workers <= 0)) return;
            
            Destroy(_preparedBuildingSprite, _preparedBuilding.GetBuildingTime());
            StartCoroutine(GameManager.Instance.AddBuilding(_preparedBuilding, Camera.main.ScreenToWorldPoint(_mousePos)));
            
            _preparedBuilding = null;
            _sprite = null;
            _preparedBuildingSprite = null;
            _isBuilding = false;
            CanBuild = false;
            
            // Debug.Log("Building Placed");
        }
    }
}