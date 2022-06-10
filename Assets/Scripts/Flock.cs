using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //Declara a variavel do Manager e a da velocidade dos peixer
    public FlockingManager myManager;
    float speed;
    bool turning = false;

    private void Start()
    {
        //Set a velocidade do peixe entre a velocidade minima e maxima pre definida no inspector
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    private void Update()
    {
        Bounds b = new Bounds(myManager.transform.position,myManager.swinLimits * 2);

        RaycastHit hit = new RaycastHit();
        Vector3 direction = myManager.transform.position - transform.position;

        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }else if(Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
        {
            turning = false;
            if (turning)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
            else
            {
                if (Random.Range(0, 100) < 10) speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
                if (Random.Range(0, 100) < 20) applyRules();
            }
            transform.Translate(0, 0, Time.deltaTime * speed);
        }


        //Chama o metodo ApplyRules
        applyRules();

        //Move o peixe
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void applyRules()
    {
        //Declara variaveis que seram usadas na rotacao dos peixes
        GameObject[] gos;
        gos = myManager.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            //Se o GameObject GO for diferente de mim(gameObject)
            if (go != this.gameObject)
            {
                //Atribui a distancia da posicao do go e de mim(gameObject) a variavel nDistance
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if(nDistance <= myManager.neighbourDistance)
                {
                    //Soma o vcentre + o posicao do GO
                    vcentre += go.transform.position;
                    //Aumenta o tamanho da grupo
                    groupSize++;

                    //Se nDistance for menor que 1
                    if(nDistance < 1.0f)
                    {
                        //Soma a minha posicao(gameObject) menos a posicao do go a variavel vavoid
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    //Cria uma variavel nova de Flock
                    Flock anotherFlock = go.GetComponent<Flock>();
                    //Soma o valor de gSpeed mais o speed do anotherFlock na variavel gSpeed
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        //Se o tamanho do grupo for maior que 0
        if(groupSize > 0)
        {
            //Divide o valor de vcentre pelo tamanho do grupo
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            //e defini o speed como o gspeed dividido pelo tamanho do grupo
            speed = gSpeed / groupSize;

            speed = Mathf.Clamp(speed, myManager.minSpeed, myManager.maxSpeed);

            //Define o valor de direction como a soma do vcentre e o vavoid menos a minha posicao
            Vector3 direction = (vcentre + vavoid) - transform.position;
            //Se a direction for diferente do Vector3 
            if (direction != Vector3.zero)
                //Ele passa uma nova rotacao
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }

    }

}
