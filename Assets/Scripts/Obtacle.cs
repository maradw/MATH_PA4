using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Obtacle : MonoBehaviour
{
    public Transform[] objetivos; 
    public AnimationCurve curvaMovimiento; 
    private bool enMovimiento = false;
    private float velocidadInicial;
    private float tiempo;
    private float aceleracion;
    private int objetivoActualIndex = 0;
    private bool moviendoseHaciaAtras = false;
    private Tween movimientoTween; 
    void Update()
    {
        if (enMovimiento)
        {
            MoverHaciaPuntoDeReferencia();
        }
    }

    void MoverHaciaPuntoDeReferencia()
    {
        // Calcular  velocidad actual
        float velocidadActual = velocidadInicial + aceleracion * Time.deltaTime;

        // Calcular  distancia al objetivo
        Vector3 objetivoPosicion = objetivos[objetivoActualIndex].position;
        float distancia = Vector3.Distance(transform.position, objetivoPosicion);

        // Cancelar cualquier tween de movimiento previo
        if (movimientoTween != null && movimientoTween.IsActive())
        {
            movimientoTween.Kill();
        }

        // Crear un tween 
        movimientoTween = transform.DOMove(objetivoPosicion, distancia / velocidadActual)
            .SetEase(curvaMovimiento) // Aplicar  curva de animación
            .OnUpdate(MostrarResultado)
            .OnComplete(CambiarObjetivo);

        Vector3 direccion = (objetivoPosicion - transform.position).normalized;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
        transform.DORotateQuaternion(rotacionObjetivo, 1f).SetEase(curvaMovimiento);
    }

    void CambiarObjetivo()
    {
        if (moviendoseHaciaAtras && objetivoActualIndex == 0)
        {
            moviendoseHaciaAtras = false;
        }
        if (!moviendoseHaciaAtras)
        {
            objetivoActualIndex = (objetivoActualIndex + 1) % objetivos.Length;
        }
        else
        {
            objetivoActualIndex = (objetivoActualIndex - 1 + objetivos.Length) % objetivos.Length;
        }
        if (objetivoActualIndex == objetivos.Length - 1)
        {
            moviendoseHaciaAtras = true;
        }
    }

    void MostrarResultado()
    {
        float tiempoClamped = Mathf.Max(tiempo, 0.1f);

        float distancia = Vector3.Distance(objetivos[objetivoActualIndex].position, transform.position);
        float resultado = distancia / tiempoClamped;
    }

    public void EstablecerMovimiento(bool estado)
    {
        enMovimiento = estado;
    }
}
