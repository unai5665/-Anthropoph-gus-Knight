using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables públicas que se pueden modificar desde el Inspector de Unity
    public float velocidad = 5f;           // La velocidad del movimiento horizontal del jugador
    public float fuerzaSalto = 10f;        // La fuerza que se aplicará al saltar
    public float longitudRaycast = 0.1f;   // La longitud del raycast que verifica si el jugador está en el suelo
    public LayerMask capaPiso;             // La capa a la que pertenece el "suelo", para que el raycast solo detecte el suelo

    // Variables privadas para el control interno del movimiento
    private bool enPiso;                  // Variable para saber si el jugador está tocando el suelo
    private Rigidbody2D rb;               // Referencia al Rigidbody2D del jugador para manipular la física

    // Start se llama una sola vez al inicio, antes de la primera actualización
    void Start()
    {
        // Obtener la referencia al componente Rigidbody2D del jugador
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update se llama una vez por frame, generalmente para controlar las entradas del jugador
    void Update()
    {
        // Obtener el input horizontal (teclas A/D o flechas) y multiplicarlo por la velocidad y el tiempo entre frames (Time.deltaTime)
        float velocidadX = Input.GetAxis("Horizontal") * Time.deltaTime * velocidad;

        // Cambiar la dirección del personaje (mirar a la izquierda o a la derecha) dependiendo de la dirección en el eje X
        if (velocidadX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);  // Voltear al personaje para que mire hacia la izquierda
        }
        if (velocidadX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);   // Voltear al personaje para que mire hacia la derecha
        }

        // Obtener la posición actual del jugador
        Vector3 posicion = transform.position;

        // Actualizar la posición del jugador en el eje X, manteniendo la posición en Y y Z sin cambios
        transform.position = new Vector3(velocidadX + posicion.x, posicion.y, posicion.z);

        // Lanzar un Raycast hacia abajo desde la posición del jugador para verificar si está tocando el suelo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaPiso);

        // Si el Raycast detecta colisión con un objeto en la capa del suelo, se establece que el jugador está en el suelo
        enPiso = hit.collider != null;

        // Si el jugador está en el suelo y presiona la tecla de salto (espacio), se aplica una fuerza hacia arriba
        if (enPiso && Input.GetKeyDown(KeyCode.Space))
        {
            // Aplica una fuerza hacia arriba al Rigidbody2D para hacer que el jugador salte
            rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
        }
    }

    // OnDrawGizmos se llama para dibujar líneas de ayuda en el editor, útil para ver visualmente el raycast
    void OnDrawGizmos()
    {
        // Establecer el color de la línea de Gizmos a rojo
        Gizmos.color = Color.red;
        
        // Dibujar una línea roja que representa el raycast desde la posición del jugador hacia abajo
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}
