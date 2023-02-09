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

    //Product ����
    //���õ� ��� ã�Ƽ� Product ���� 
    //Product�� �˾Ƽ� Rack�� ������ �ı� ��
    //MOS�� ���� �� agent���� Product�� ���� �Ǿ��ٴ� �޽��� ������

    //Product ����
    //3,4�� ��忡 Product�� ���� ���
    //�˾Ƽ� üũ�ؼ� Product ����
    //����� MOS�� ���� �˸� �ʿ� ����


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
