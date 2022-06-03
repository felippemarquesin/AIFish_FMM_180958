using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    //Declara a variavel da prefab do baixo, a variavel da quantidade de peixes, o array dos peixes e o limite da area que os peixes vao estar
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);

    //Declara a variavel da velocidade minima e maxima do peixe, a distancia dos peixes e a velocidade da rotação deles
    [Header("Configurações do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f,5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(1.0f, 5.0f)]
    public float rotationSpeed;

    private void Start()
    {
        allFish = new GameObject[numFish];
        for(int i = 0; i < numFish; i++)
        {
            //Defini uma posicao aleatoria dentro dos parametros antes definido
            Vector3 pos =  this.transform.position + new Vector3(Random.Range(-swinLimits.x,swinLimits.x), 
                Random.Range(-swinLimits.y, swinLimits.y), Random.Range(-swinLimits.z,swinLimits.z));

            //Instancia os peixes de acordo com o numero no array
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
        }
    }
}
