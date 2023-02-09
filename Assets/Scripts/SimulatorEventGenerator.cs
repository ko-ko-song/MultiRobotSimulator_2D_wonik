using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatorEventGenerator : MonoBehaviour
{
    public GameObject environmentManager;
    

    private void Awake()
    {
        environmentManager = GameObject.Find("EnvironmentManager");
        Init();
    }

    //Product 생성
    //선택된 노드 찾아서 Product 생성 
    //Product는 알아서 Rack이 없으면 파괴 됨
    //MOS를 통해 각 agent에게 Product가 생성 되었다는 메시지 보내기

    //Product 제거
    //3,4번 노드에 Product가 있을 경우
    //알아서 체크해서 Product 제거
    //현재는 MOS를 통해 알릴 필요 없음


    public bool CreateProduct(string nodeId)
    {
        return environmentManager.GetComponent<EnvironmentManager>().CreateProduct(nodeId);
    }
    
    

    private GameObject vertex3Obj;
    private GameObject vertex4Obj;
    
    private void Init()
    {
        EnvironmentObject vertex3 = environmentManager.GetComponent<EnvironmentManager>().getVertex("3");
        EnvironmentObject vertex4 = environmentManager.GetComponent<EnvironmentManager>().getVertex("4");

        if (vertex3 == null)
        {
            Invoke("Init", 1f);
            return;
        }

        this.vertex3Obj = vertex3.gameObject;
        this.vertex4Obj = vertex4.gameObject;
        DeleteProduct();
    }
    private void DeleteProduct()
    {
        GameObject[] products = GameObject.FindGameObjectsWithTag("Product");
        foreach(GameObject product in products)
        {
            if (Mathf.Abs(product.transform.position.x - vertex3Obj.transform.position.x) < 0.2f
                && Mathf.Abs(product.transform.position.y - vertex3Obj.transform.position.y) < 0.2f)
            {
                Destroy(product);
            }
            else if(Mathf.Abs(product.transform.position.x - vertex4Obj.transform.position.x) < 0.2f
                && Mathf.Abs(product.transform.position.y - vertex4Obj.transform.position.y) < 0.2f)
            {
                Destroy(product);
            }
        }
        Invoke("DeleteProduct", 5.0f);
    }

}
