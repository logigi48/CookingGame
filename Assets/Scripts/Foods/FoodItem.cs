using UnityEngine;

public class FoodItem : MonoBehaviour, Interfaces.IInteractable
{
    // --- Interface Requirements ---
    public bool Grabable { get; set; } = true;
    public bool Grabed { get; set; } = false;
    public void Action() { } // Still blank, food doesn't respond to button clicks
    // ------------------------------

    [Header("Data")]
    public FoodData currentData;
    private GameObject currentMesh;

    [Header("Physics")]
    public Rigidbody rb;
    public float extraGravity = 5f; // Kept your original gravity mechanic

    // State Tracking
    private FoodAction currentAction = FoodAction.None;
    private float currentProgress = 0f;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        SpawnMesh();
    }

    private void FixedUpdate()
    {
        // Using FixedUpdate is best practice for adding physics forces
        rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
    }

    /// <summary>
    /// Appliances (Pans, Knives, Friers) will call this method!
    /// </summary>
    public void ApplyProcess(FoodAction action, float amount)
    {
        // If the appliance is doing something new (e.g., moved from pan to frier), reset progress
        if (currentAction != action)
        {
            currentAction = action;
            currentProgress = 0f;
        }

        // Ask our data if this action is allowed
        FoodRecipe recipe = currentData.GetRecipeForAction(action);

        if (recipe != null)
        {
            currentProgress += amount;

            // Did we finish cooking/slicing?
            if (currentProgress >= recipe.requiredProgress)
            {
                TransitionToNextStage(recipe.nextStage);
            }
        }
        // If recipe is null, it means you tried to do something invalid 
        // (like boiling a burnt steak), so the script safely ignores it!
    }

    private void TransitionToNextStage(FoodData nextData)
    {
        currentData = nextData;
        currentProgress = 0f;
        currentAction = FoodAction.None;
        
        SpawnMesh();
    }

    private void SpawnMesh()
    {
        if (currentMesh != null) 
        {
            Destroy(currentMesh);
        }

        if (currentData != null && currentData.meshPrefab != null)
        {
            currentMesh = Instantiate(currentData.meshPrefab, transform.position, transform.rotation, this.transform);
        }
    }
}