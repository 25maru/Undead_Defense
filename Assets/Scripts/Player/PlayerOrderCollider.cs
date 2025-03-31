using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrderCollider : MonoBehaviour
{
    [SerializeField] SphereCollider orderCollider;
    [SerializeField] GameObject circle;
    [SerializeField] List<Soldier> orderSoldiers;
    [SerializeField] GameObject orderPositionPrefab;
    [SerializeField] Transform orderPosition;
    bool isOrder;

    private void Awake()
    {
        orderCollider.enabled = false;
        circle.SetActive(false);
    }
    private void Update()
    {
        if (isOrder)
        {
            orderPosition.position = transform.position;
        }
    }
    public void StartOrder()
    {
        for (int i = 0; i < orderSoldiers.Count; i++)
        {
            orderSoldiers[i].EndOrder();
        }
        orderSoldiers.Clear();
        if(orderPosition == null)
        {
            orderPosition = Instantiate(orderPositionPrefab).transform;
        }

        isOrder = true;
        orderCollider.enabled=true;
        circle.SetActive(true);
        Collider[] colliders = Physics.OverlapSphere(transform.position,orderCollider.radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].TryGetComponent<Soldier>(out var soldier))
            {
                orderSoldiers.Add(soldier);
                soldier.GetOrder(orderPosition);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Soldier>(out var soldier) && !orderSoldiers.Contains(soldier))
        {
            orderSoldiers.Add(soldier);
            soldier.GetOrder(orderPosition);
        }
    }
    public void EndOrder()
    {
        isOrder = false;
        orderCollider.enabled = false;
        circle.SetActive(false);
        for(int i = 0;i < orderSoldiers.Count; i++)
        {
            orderSoldiers[i].MoveOrder();
        }
    }
    public void Hold()
    {
        for (int i = 0; i < orderSoldiers.Count; i++)
        {
            orderSoldiers[i].Hold();
        }
    }
}
