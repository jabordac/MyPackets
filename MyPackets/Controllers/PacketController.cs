using MyPackets.Models;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MyNeighborhood.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PacketController : ControllerBase
    {
        public PacketController()
        {
        }

        [HttpGet]
        public IActionResult Get(PacketRequest packetRequest)
        {
            int idPaquete;
            int unidadesPaquete;

            int idPaqueteInicial = 0;
            int unidadesPaqueteInicial = 0;

            int idPaqueteFinal = 0;
            int unidadesPaqueteFinal = 0;

            int cantidadPaquetes = 0;

            if (packetRequest.tamanioCamion <= 0 || packetRequest.lstPaquetes == null)
            {
                return BadRequest("Tamaño y lista de paquetes son requeridos.");
            }

            int limiteCamion = packetRequest.tamanioCamion - 30;
            int totalCamion = 0;

            List<int> lstPaquetesInicial = packetRequest.lstPaquetes.OrderByDescending(x => x).Where(x => x < limiteCamion) .ToList();
            List<int> lstPaquetesFinal = new List<int>();

            for (idPaquete = 0; idPaquete <= (packetRequest.lstPaquetes.Count - 1); idPaquete++)
            {
                unidadesPaquete = lstPaquetesInicial[idPaquete];

                if(cantidadPaquetes == 0)
                {
                    idPaqueteInicial = idPaquete;
                    unidadesPaqueteInicial = unidadesPaquete;
                }

                if ((totalCamion + unidadesPaquete) <= limiteCamion)
                {
                    if(cantidadPaquetes <= 1 || (cantidadPaquetes == 2 && unidadesPaqueteFinal == unidadesPaquete))
                    {
                        totalCamion += unidadesPaquete;
                        lstPaquetesFinal.Add(unidadesPaquete);

                        idPaqueteFinal = idPaquete;
                        unidadesPaqueteFinal = unidadesPaquete;

                        cantidadPaquetes++;
                    }

                    if (idPaquete == (packetRequest.lstPaquetes.Count - 1)) // Llega al final sin completar límite del camión
                    {
                        if (cantidadPaquetes == 1)
                        {
                            lstPaquetesFinal = new List<int>();
                            idPaquete = idPaqueteInicial + 1;
                            totalCamion = 0;
                        }
                    }
                }

                if(totalCamion == limiteCamion)
                {
                    break;
                }
            }

            if (totalCamion < limiteCamion)
            {
                return BadRequest("No se encontraron combinaciones válidas.");
            }

            return Ok(lstPaquetesFinal);
        }

    }
}
