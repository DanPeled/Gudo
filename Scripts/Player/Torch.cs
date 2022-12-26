using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject drop;
    void OnMouseDown()
    {
        if(GameObject.Find("Player").GetComponent<Movement>().playerActive){
            GameObject drop = Instantiate(this.drop);
            drop.transform.position = transform.position;
            transform.position = new Vector3(100000,100000,1);           
        }
    }
}