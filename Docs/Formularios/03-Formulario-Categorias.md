# Categorías

1. **Nombre:** Categorías.
2. **Objetivo:** consultar categorías.
3. **Usuario:** administrador o vendedor autorizado.
4. **Campos:** búsqueda, selección y datos propios visibles del módulo.
5. **Controles:** Form, Label, TextBox/ComboBox/NumericUpDown según corresponda, Button y DataGridView; pestañas nativas en reportes.
6. **Obligatorios:** las selecciones y valores necesarios para confirmar la operación.
7. **Validaciones:** requeridos, importes/cantidades positivos, unicidad y reglas de saldo o stock aplicables.
8. **Botones:** consulta/actualización y acciones explícitas del módulo; cerrar mediante la ventana.
9. **Flujo:** abrir desde el menú → cargar SQLite → seleccionar o ingresar → validar → confirmar → refrescar.
10. **Errores:** mensajes en español, por ejemplo “Stock insuficiente” o “El importe debe ser mayor que cero”.
11. **Resultado esperado:** operación persistida y grilla actualizada sin datos inconsistentes.
12. **Wireframe:** `[Título/Filtros] [Acciones]` / `[Tabla de resultados]` / `[Confirmar/Cerrar]`.
13. **Clase:** `CrudForm`.
14. **Controlador:** `AppController`.
15. **Servicio:** `TechStoreDbContext`.
