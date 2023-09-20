/*
Este archivo es un controlador en una aplicación ASP.NET Core MVC que maneja las operaciones CRUD (Crear, Leer, Actualizar y Eliminar) para la entidad "Usuario". 
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCCRUD.Models;

namespace MVCCRUD.Controllers
{
  // Declaración del controlador UsuariosController
  public class UsuariosController : Controller
  {
    private readonly MVCCRUDContext _context;

    // Constructor que recibe una instancia de MVCCRUDContext
    public UsuariosController(MVCCRUDContext context)
    {
      _context = context;
    }

    // GET: Usuarios (Muestra una lista de usuarios)
    public async Task<IActionResult> Index()
    {
      // Verifica si la entidad 'MVCCRUDContext.Usuarios' no es nula
      return _context.Usuarios != null ?
                    View(await _context.Usuarios.ToListAsync()) : // Devuelve una vista con la lista de usuarios
                    Problem("Entity set 'MVCCRUDContext.Usuarios' is null."); // Devuelve un problema si la entidad es nula
    }

    // GET: Usuarios/Details/{id} (Muestra los detalles de un usuario por su ID)
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null || _context.Usuarios == null)
      {
        return NotFound(); // Devuelve un error 404 si el ID es nulo o la entidad es nula
      }

      // Busca un usuario por su ID en la base de datos
      var usuario = await _context.Usuarios
          .FirstOrDefaultAsync(m => m.Id == id);
      if (usuario == null)
      {
        return NotFound(); // Devuelve un error 404 si el usuario no se encuentra
      }

      return View(usuario); // Devuelve la vista con los detalles del usuario
    }

    // GET: Usuarios/Create (Muestra la vista para crear un nuevo usuario)
    public IActionResult Create()
    {
      return View(); // Devuelve la vista de creación de usuario
    }

    // POST: Usuarios/Create (Procesa la creación de un nuevo usuario)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Fecha,Clave")] Usuario usuario)
    {
      if (ModelState.IsValid)
      {
        // Agrega un nuevo usuario a la base de datos
        _context.Add(usuario);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index)); // Redirige a la acción Index
      }
      return View(usuario); // Devuelve la vista de creación de usuario si hay errores de validación
    }

    // GET: Usuarios/Edit/{id} (Muestra la vista para editar un usuario por su ID)
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null || _context.Usuarios == null)
      {
        return NotFound(); // Devuelve un error 404 si el ID es nulo o la entidad es nula
      }

      // Busca un usuario por su ID en la base de datos
      var usuario = await _context.Usuarios.FindAsync(id);
      if (usuario == null)
      {
        return NotFound(); // Devuelve un error 404 si el usuario no se encuentra
      }
      return View(usuario); // Devuelve la vista de edición de usuario
    }

    // POST: Usuarios/Edit (Procesa la edición de un usuario)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Fecha,Clave")] Usuario usuario)
    {
      if (id != usuario.Id)
      {
        return NotFound(); // Devuelve un error 404 si el ID no coincide con el usuario
      }

      if (ModelState.IsValid)
      {
        try
        {
          // Actualiza el usuario en la base de datos
          _context.Update(usuario);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!UsuarioExists(usuario.Id))
          {
            return NotFound(); // Devuelve un error 404 si el usuario no se encuentra
          }
          else
          {
            throw;
          }
        }
        return RedirectToAction(nameof(Index)); // Redirige a la acción Index
      }
      return View(usuario); // Devuelve la vista de edición de usuario si hay errores de validación
    }

    // GET: Usuarios/Delete/{id} (Muestra la vista para eliminar un usuario por su ID)
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null || _context.Usuarios == null)
      {
        return NotFound(); // Devuelve un error 404 si el ID es nulo o la entidad es nula
      }

      // Busca un usuario por su ID en la base de datos
      var usuario = await _context.Usuarios
          .FirstOrDefaultAsync(m => m.Id == id);
      if (usuario == null)
      {
        return NotFound(); // Devuelve un error 404 si el usuario no se encuentra
      }
      return View(usuario); // Devuelve la vista de eliminación de usuario
    }

    // POST: Usuarios/DeleteConfirmed (Procesa la eliminación confirmada de un usuario)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      if (_context.Usuarios == null)
      {
        return Problem("Entity set 'MVCCRUDContext.Usuarios' is null"); // Devuelve un problema si la entidad es nula
      }

      // Busca un usuario por su ID en la base de datos y lo elimina
      var usuario = await _context.Usuarios.FindAsync(id);
      if (usuario != null)
      {
        _context.Usuarios.Remove(usuario);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index)); // Redirige a la acción Index
    }

    // Verifica si un usuario con un ID dado existe en la base de datos
    private bool UsuarioExists(int id)
    {
      return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
