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
    private int objetivoActualIndex = 0;
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
        // Obtener y limitar los valores de entrada
        // Calcular  velocidad actual
        float velocidadActual = velocidadInicial * Time.deltaTime;

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
            .SetEase(curvaMovimiento); // Aplicar  curva de animación

        Vector3 direccion = (objetivoPosicion - transform.position).normalized;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
        transform.DORotateQuaternion(rotacionObjetivo, 1f).SetEase(curvaMovimiento);
    }
   public void EstablecerMovimiento(bool estado)
    {
        enMovimiento = estado;
    }
}
