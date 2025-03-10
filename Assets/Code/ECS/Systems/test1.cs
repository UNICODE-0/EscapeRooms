using System;
using UnityEngine;

public class test1 : MonoBehaviour
{
 [SerializeField] private Transform _target; // Цель, за которой следует объект
    [SerializeField] private Transform _center; // Центр сферы (задается через инспектор)
    [SerializeField] private float _speed = 5f; // Скорость движения по сфере
    [SerializeField] private float _radius = 2f; // Радиус сферы
    
    [SerializeField] private float _rotSpeed = 5f; // Скорость движения по сфере


    private float _currentAzimuth; // Азимутальный угол (горизонтальный)
    private float _currentZenith;  // Зенитный угол (вертикальный)

    private void Start()
    {
        // Инициализация углов
        _currentAzimuth = 0f; // Начальный азимутальный угол
        _currentZenith = Mathf.PI / 2; // Начальный зенитный угол (на "экваторе")
    }

    private void Update()
    {
        if (_target == null || _center == null)
        {
            Debug.LogWarning("Target или Center не заданы!");
            return;
        }

        // Вычисляем направление к цели относительно центра сферы
        Vector3 toTarget = (_target.position - _center.position).normalized;

        // Преобразуем позицию цели в сферические координаты
        float targetAzimuth = Mathf.Atan2(toTarget.z, toTarget.x); // Азимутальный угол
        float targetZenith = Mathf.Acos(Mathf.Clamp(toTarget.y, -1f, 1f)); // Зенитный угол

        // Угловая скорость
        float angularSpeed = _speed / _radius;

        // Плавно изменяем текущие углы к целевым
        _currentAzimuth = Mathf.MoveTowardsAngle(
            _currentAzimuth * Mathf.Rad2Deg,
            targetAzimuth * Mathf.Rad2Deg,
            angularSpeed * Mathf.Rad2Deg * Time.deltaTime
        ) * Mathf.Deg2Rad;

        _currentZenith = Mathf.MoveTowards(
            _currentZenith,
            targetZenith,
            angularSpeed * Time.deltaTime
        );

        // Ограничиваем зенитный угол, чтобы избежать выхода за полюса
        _currentZenith = Mathf.Clamp(_currentZenith, 0.01f, Mathf.PI - 0.01f);

        // Переводим сферические координаты обратно в декартовы
        float sinZenith = Mathf.Sin(_currentZenith);
        float x = _radius * sinZenith * Mathf.Cos(_currentAzimuth);
        float y = _radius * Mathf.Cos(_currentZenith);
        float z = _radius * sinZenith * Mathf.Sin(_currentAzimuth);

        // Обновляем позицию объекта относительно центра сферы
        //transform.position = Vector3.MoveTowards(transform.position, _center.position + new Vector3(x, y, z), _speed * Time.deltaTime);
        transform.position = _center.position + new Vector3(x, y, z);
        transform.rotation = Quaternion.Lerp(transform.rotation, _target.rotation, Time.deltaTime * _rotSpeed);
    }
}
