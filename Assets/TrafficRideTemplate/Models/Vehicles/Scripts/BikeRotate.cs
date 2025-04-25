using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BikeRotate : MonoBehaviour
{

    public float rotateSpeed = 10.0f;
    public Text BikeNameUI;
    public Transform root;

    public GameObject[] bikes;

    private int currentBike = 0;
    private Vector3 position;

    public void Right()
    {
        currentBike--;
        if (currentBike < 0) { currentBike = bikes.Length - 1; }
        ActiveCurrentBike(currentBike);
    }


    public void Left()
    {
        currentBike++;
        if (currentBike > bikes.Length - 1) { currentBike = 0; }
        ActiveCurrentBike(currentBike);
    }

    public void ActiveCurrentBike(int current)
    {
        int count = 0;
        float polys = 0;

        foreach (GameObject bike in bikes)
        {
            if (currentBike == count)
            {
                BikeNameUI.text = "Motorbike " + (currentBike + 1).ToString();
                foreach (Transform Object in bikes[currentBike].transform)
                    polys += Object.GetComponent<MeshFilter>().mesh.triangles.Length / 3.0f;

            }
            else
            {
                bikes[count].transform.rotation = Quaternion.Euler(0, 0, 0);

            }

            count++;
        }
    }



    public void Start()
    {
        position = root.position;
    }

    void Update()
    {
        bikes[currentBike].transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        root.position = Vector3.Lerp(root.position, position + new Vector3(bikes[currentBike].transform.position.x, 0, bikes[currentBike].transform.position.z), Time.deltaTime * 10.0f);
    }


}
