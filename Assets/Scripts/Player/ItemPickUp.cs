using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();

        if (item != null)
        {
            // Get item details
            //Debug.Log(item.ItemCode);
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);
            if (itemDetails.canBePickedUp == true)
            {
                //Debug.Log(itemDetails.itemDescription);

                InventoryManager.Instance.AddItem(InventoryLocation.player, item, collision.gameObject);
            }
        }
    }
}
