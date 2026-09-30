using System;
using System.IO;
using System.Windows.Forms;

namespace pk3DS.WinForms;

public partial class RSTE
{
    private const int PorcentajeBaseNivel = 100;
    private const int PorcentajeAumentoNivel = 17;
    private const int FactorPorcentualNivel = PorcentajeBaseNivel + PorcentajeAumentoNivel;
    private const ushort NivelMaximoEntrenador = 100;
    private const int PosicionNivelPokemon = 2;
    private const int LongitudNivelPokemon = sizeof(ushort);
    private const int PosicionEspeciePokemonNivel = 4;
    private const int LongitudBasePokemonEntrenador = 8;
    private const int LongitudObjetoPokemonEntrenador = sizeof(ushort);
    private const int LongitudMovimientosPokemonEntrenador = 4 * sizeof(ushort);
    private const int LongitudDatosEntrenadorORAS = 24;
    private const int LongitudDatosEntrenadorXY = 20;
    private const int PosicionCantidadPokemonORASNivel = 7;
    private const int PosicionCantidadPokemonXYNivel = 3;
    private const int MascaraMovimientosEntrenador = 1;
    private const int MascaraObjetosEntrenador = 2;

    private void B_SubirNiveles_Click(object sender, EventArgs e)
    {
        EjecutarSubidaNiveles();
    }

    private void EjecutarSubidaNiveles()
    {
        if (!ConfirmarSubidaNiveles())
            return;

        ResultadoSubidaNiveles resultado;
        try
        {
            resultado = AplicarSubidaNivelesAtomica();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, "No se aplicó el aumento de niveles: " + ex.Message,
                "Error al aumentar niveles", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Exception errorActualizacion = null;
        if (index > 0)
        {
            try
            {
                ReadFile();
            }
            catch (Exception ex)
            {
                errorActualizacion = ex;
            }
        }

        int sinCambioPorRedondeo = resultado.PokemonProcesados - resultado.PokemonAumentados -
            resultado.PokemonYaEnNivelMaximo;
        string resumen =
            "Los niveles se aumentaron correctamente.\r\n\r\n" +
            $"Entrenadores procesados: {resultado.EntrenadoresProcesados}\r\n" +
            $"Pokémon procesados: {resultado.PokemonProcesados}\r\n" +
            $"Pokémon que aumentaron: {resultado.PokemonAumentados}\r\n" +
            $"Sin cambio por redondeo: {sinCambioPorRedondeo}\r\n" +
            $"Pokémon que alcanzaron el nivel 100: {resultado.PokemonQueAlcanzaronNivelMaximo}\r\n" +
            $"Pokémon que ya estaban en el nivel 100: {resultado.PokemonYaEnNivelMaximo}";

        if (errorActualizacion is not null)
        {
            resumen += "\r\n\r\nLos datos se aplicaron, pero no se pudo actualizar la pantalla: " +
                errorActualizacion.Message;
        }

        MessageBox.Show(this, resumen, "Niveles +17%", MessageBoxButtons.OK,
            errorActualizacion is null ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
    }

    private bool ConfirmarSubidaNiveles()
    {
        const string mensaje =
            "Esta función aumentará un 17 % el nivel de todos los Pokémon de todos los entrenadores.\r\n\r\n" +
            "• El resultado se redondeará al entero más cercano.\r\n" +
            "• Los niveles 1 y 2 pueden permanecer iguales debido al redondeo.\r\n" +
            "• Ningún Pokémon superará el nivel 100.\r\n" +
            "• Debe utilizarse después de «Mejorar entrenadores» y «Chetar Rival».\r\n\r\n" +
            "Esta operación es acumulativa: si la ejecutas otra vez, volverá a aumentar\r\n" +
            "los niveles resultantes y no podrá deshacerse desde este editor.\r\n\r\n" +
            "¿Quieres continuar?";

        DialogResult respuesta = MessageBox.Show(this, mensaje, "Niveles +17%",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        return respuesta == DialogResult.Yes;
    }

    /// <summary>
    /// Aumenta un nivel un 17 %, redondea al entero más cercano y respeta el límite 100.
    /// </summary>
    private static ushort CalcularNivelAumentado(ushort nivelActual)
    {
        if (nivelActual is 0 or > NivelMaximoEntrenador)
        {
            throw new ArgumentOutOfRangeException(nameof(nivelActual), nivelActual,
                $"El nivel debe estar comprendido entre 1 y {NivelMaximoEntrenador}.");
        }

        // Sumar la mitad del divisor aplica un redondeo aritmético sin usar coma flotante.
        int nivelCalculado = ((nivelActual * FactorPorcentualNivel) + (PorcentajeBaseNivel / 2)) /
            PorcentajeBaseNivel;
        return (ushort)Math.Min(nivelCalculado, NivelMaximoEntrenador);
    }

    /// <summary>
    /// Prepara, valida y confirma atómicamente el aumento sobre los equipos abiertos.
    /// La actualización visual se realiza después, desde el flujo de interfaz.
    /// </summary>
    private ResultadoSubidaNiveles AplicarSubidaNivelesAtomica()
    {
        if (!Main.Config.ORAS)
        {
            throw new InvalidOperationException(
                "El aumento global de niveles solo está disponible para Rubí Omega y Zafiro Alfa.");
        }

        // Conserva cualquier cambio que el usuario haya hecho en el entrenador visible.
        if (index > 0)
            WriteFile();

        ValidarEquiposParaSubidaNiveles(trdata, trpoke, oras: true);

        byte[][] datosAnteriores = ClonarArchivosParaSubidaNiveles(trdata);
        byte[][] equiposAnteriores = ClonarArchivosParaSubidaNiveles(trpoke);
        byte[][] datosTrabajo = ClonarArchivosParaSubidaNiveles(datosAnteriores);
        byte[][] equiposTrabajo = ClonarArchivosParaSubidaNiveles(equiposAnteriores);
        var resultado = new ResultadoSubidaNiveles();

        AplicarAumentoNiveles(datosTrabajo, equiposTrabajo, oras: true, resultado);
        ValidarAumentoNiveles(datosAnteriores, equiposAnteriores,
            datosTrabajo, equiposTrabajo, oras: true, resultado);

        // Hasta este punto solo se han modificado copias. La sustitución se realiza después de validar todo.
        Array.Copy(equiposTrabajo, trpoke, trpoke.Length);
        return resultado;
    }

    private static byte[][] ClonarArchivosParaSubidaNiveles(byte[][] origen)
    {
        ArgumentNullException.ThrowIfNull(origen);

        byte[][] copia = new byte[origen.Length][];
        for (int i = 0; i < origen.Length; i++)
        {
            byte[] entrada = origen[i];
            copia[i] = entrada is null ? null : (byte[])entrada.Clone();
        }

        return copia;
    }

    /// <summary>
    /// Comprueba que todos los equipos pueden recorrerse de forma segura sin modificar sus datos.
    /// </summary>
    private static void ValidarEquiposParaSubidaNiveles(byte[][] datos, byte[][] equipos, bool oras)
    {
        ArgumentNullException.ThrowIfNull(datos);
        ArgumentNullException.ThrowIfNull(equipos);

        if (datos.Length != equipos.Length)
            throw new InvalidDataException("Los datos y los equipos no tienen la misma cantidad de entradas.");
        if (datos.Length < 2)
            throw new InvalidDataException("No hay entrenadores utilizables para aumentar sus niveles.");

        int longitudMinimaDatos = oras ? LongitudDatosEntrenadorORAS : LongitudDatosEntrenadorXY;
        int posicionCantidad = oras ? PosicionCantidadPokemonORASNivel : PosicionCantidadPokemonXYNivel;

        // La entrada 0 es de relleno; los entrenadores utilizables comienzan en 1.
        for (int id = 1; id < datos.Length; id++)
        {
            byte[] datosEntrenador = datos[id]
                ?? throw new InvalidDataException($"Los datos del entrenador {id} son nulos.");
            byte[] equipoEntrenador = equipos[id]
                ?? throw new InvalidDataException($"El equipo del entrenador {id} es nulo.");

            if (datosEntrenador.Length < longitudMinimaDatos)
                throw new InvalidDataException($"Los datos del entrenador {id} están incompletos.");

            int cantidad = datosEntrenador[posicionCantidad];
            if (cantidad is < 1 or > 6)
            {
                throw new InvalidDataException(
                    $"El entrenador {id} declara una cantidad inválida de Pokémon: {cantidad}.");
            }

            int formato = oras ? BitConverter.ToUInt16(datosEntrenador, 0) : datosEntrenador[0];
            int longitudPokemon = ObtenerLongitudPokemonEntrenador(formato);
            int longitudEsperada = cantidad * longitudPokemon;
            if (equipoEntrenador.Length != longitudEsperada)
            {
                throw new InvalidDataException(
                    $"El equipo del entrenador {id} ocupa {equipoEntrenador.Length} bytes; " +
                    $"se esperaban {longitudEsperada}.");
            }

            for (int posicionEquipo = 0, numeroPokemon = 1;
                 posicionEquipo < equipoEntrenador.Length;
                 posicionEquipo += longitudPokemon, numeroPokemon++)
            {
                int posicionNivel = posicionEquipo + PosicionNivelPokemon;
                if (posicionNivel + LongitudNivelPokemon > equipoEntrenador.Length)
                {
                    throw new InvalidDataException(
                        $"El nivel del Pokémon {numeroPokemon} del entrenador {id} está incompleto.");
                }

                ushort nivel = BitConverter.ToUInt16(equipoEntrenador, posicionNivel);
                ushort especie = BitConverter.ToUInt16(
                    equipoEntrenador, posicionEquipo + PosicionEspeciePokemonNivel);
                if (especie == 0)
                {
                    // ORAS conserva algunas ranuras vacías dentro de la cantidad declarada.
                    // No son Pokémon y deben permanecer exactamente con nivel 0.
                    if (nivel != 0)
                    {
                        throw new InvalidDataException(
                            $"La ranura vacía {numeroPokemon} del entrenador {id} tiene nivel {nivel}.");
                    }
                    continue;
                }

                if (nivel is 0 or > NivelMaximoEntrenador)
                {
                    throw new InvalidDataException(
                        $"El Pokémon {numeroPokemon} del entrenador {id} tiene un nivel inválido: {nivel}.");
                }
            }
        }
    }

    private static int ObtenerLongitudPokemonEntrenador(int formato)
    {
        int longitud = LongitudBasePokemonEntrenador;
        if ((formato & MascaraObjetosEntrenador) != 0)
            longitud += LongitudObjetoPokemonEntrenador;
        if ((formato & MascaraMovimientosEntrenador) != 0)
            longitud += LongitudMovimientosPokemonEntrenador;
        return longitud;
    }

    /// <summary>
    /// Aumenta los niveles únicamente en los equipos recibidos, que deben ser copias de trabajo.
    /// </summary>
    private static void AplicarAumentoNiveles(
        byte[][] datos, byte[][] equipos, bool oras, ResultadoSubidaNiveles resultado)
    {
        ArgumentNullException.ThrowIfNull(resultado);
        ValidarEquiposParaSubidaNiveles(datos, equipos, oras);

        int posicionCantidad = oras ? PosicionCantidadPokemonORASNivel : PosicionCantidadPokemonXYNivel;

        for (int id = 1; id < datos.Length; id++)
        {
            byte[] datosEntrenador = datos[id];
            byte[] equipoEntrenador = equipos[id];
            int cantidad = datosEntrenador[posicionCantidad];
            int formato = oras ? BitConverter.ToUInt16(datosEntrenador, 0) : datosEntrenador[0];
            int longitudPokemon = ObtenerLongitudPokemonEntrenador(formato);

            resultado.EntrenadoresProcesados++;
            for (int pokemon = 0; pokemon < cantidad; pokemon++)
            {
                int posicionNivel = (pokemon * longitudPokemon) + PosicionNivelPokemon;
                int posicionEspecie = (pokemon * longitudPokemon) + PosicionEspeciePokemonNivel;
                if (BitConverter.ToUInt16(equipoEntrenador, posicionEspecie) == 0)
                    continue;

                ushort nivelAnterior = BitConverter.ToUInt16(equipoEntrenador, posicionNivel);
                resultado.PokemonProcesados++;

                if (nivelAnterior == NivelMaximoEntrenador)
                {
                    resultado.PokemonYaEnNivelMaximo++;
                    continue;
                }

                ushort nivelNuevo = CalcularNivelAumentado(nivelAnterior);
                if (nivelNuevo == nivelAnterior)
                    continue;

                equipoEntrenador[posicionNivel] = (byte)nivelNuevo;
                equipoEntrenador[posicionNivel + 1] = (byte)(nivelNuevo >> 8);
                resultado.PokemonAumentados++;

                if (nivelNuevo == NivelMaximoEntrenador)
                    resultado.PokemonQueAlcanzaronNivelMaximo++;
            }
        }
    }

    /// <summary>
    /// Comprueba de forma independiente que la transformación solo haya modificado niveles.
    /// </summary>
    private static void ValidarAumentoNiveles(
        byte[][] datosAnteriores, byte[][] equiposAnteriores,
        byte[][] datosModificados, byte[][] equiposModificados,
        bool oras, ResultadoSubidaNiveles resultado)
    {
        ArgumentNullException.ThrowIfNull(datosAnteriores);
        ArgumentNullException.ThrowIfNull(equiposAnteriores);
        ArgumentNullException.ThrowIfNull(datosModificados);
        ArgumentNullException.ThrowIfNull(equiposModificados);
        ArgumentNullException.ThrowIfNull(resultado);

        bool cantidadesCoinciden = datosAnteriores.Length == equiposAnteriores.Length &&
            datosModificados.Length == equiposModificados.Length &&
            datosAnteriores.Length == datosModificados.Length;
        if (!cantidadesCoinciden)
            throw new InvalidDataException("El aumento de niveles cambió la cantidad de entradas.");

        ValidarEquiposParaSubidaNiveles(datosAnteriores, equiposAnteriores, oras);
        ValidarEquiposParaSubidaNiveles(datosModificados, equiposModificados, oras);

        if (!BytesIgualesNivel(datosAnteriores[0], datosModificados[0]) ||
            !BytesIgualesNivel(equiposAnteriores[0], equiposModificados[0]))
        {
            throw new InvalidDataException("El aumento de niveles modificó la entrada 0 de relleno.");
        }

        int entrenadoresEsperados = 0;
        int pokemonEsperados = 0;
        int aumentadosEsperados = 0;
        int alcanzaronMaximoEsperados = 0;
        int yaEnMaximoEsperados = 0;
        int posicionCantidad = oras ? PosicionCantidadPokemonORASNivel : PosicionCantidadPokemonXYNivel;

        for (int id = 1; id < datosAnteriores.Length; id++)
        {
            byte[] datosOriginales = datosAnteriores[id];
            byte[] datosFinales = datosModificados[id];
            byte[] equipoOriginal = equiposAnteriores[id];
            byte[] equipoFinal = equiposModificados[id];

            if (!datosOriginales.AsSpan().SequenceEqual(datosFinales))
                throw new InvalidDataException($"El aumento de niveles modificó los datos del entrenador {id}.");
            if (equipoOriginal.Length != equipoFinal.Length)
                throw new InvalidDataException($"El aumento de niveles cambió el tamaño del equipo {id}.");

            int cantidad = datosOriginales[posicionCantidad];
            int formato = oras ? BitConverter.ToUInt16(datosOriginales, 0) : datosOriginales[0];
            int longitudPokemon = ObtenerLongitudPokemonEntrenador(formato);
            entrenadoresEsperados++;

            for (int pokemon = 0; pokemon < cantidad; pokemon++)
            {
                int inicioPokemon = pokemon * longitudPokemon;
                int posicionNivel = inicioPokemon + PosicionNivelPokemon;

                for (int posicionRelativa = 0; posicionRelativa < longitudPokemon; posicionRelativa++)
                {
                    bool esByteDeNivel = posicionRelativa >= PosicionNivelPokemon &&
                        posicionRelativa < PosicionNivelPokemon + LongitudNivelPokemon;
                    int posicionAbsoluta = inicioPokemon + posicionRelativa;
                    if (!esByteDeNivel && equipoOriginal[posicionAbsoluta] != equipoFinal[posicionAbsoluta])
                    {
                        throw new InvalidDataException(
                            $"El aumento de niveles modificó el byte {posicionRelativa} del Pokémon " +
                            $"{pokemon + 1} del entrenador {id}.");
                    }
                }

                ushort nivelAnterior = BitConverter.ToUInt16(equipoOriginal, posicionNivel);
                ushort nivelFinal = BitConverter.ToUInt16(equipoFinal, posicionNivel);
                ushort especie = BitConverter.ToUInt16(
                    equipoOriginal, inicioPokemon + PosicionEspeciePokemonNivel);
                if (especie == 0)
                {
                    if (nivelFinal != nivelAnterior)
                    {
                        throw new InvalidDataException(
                            $"La subida de niveles modificó la ranura vacía {pokemon + 1} " +
                            $"del entrenador {id}.");
                    }
                    continue;
                }

                ushort nivelEsperado = CalcularNivelEsperadoParaValidacion(nivelAnterior);
                if (nivelFinal != nivelEsperado)
                {
                    throw new InvalidDataException(
                        $"El Pokémon {pokemon + 1} del entrenador {id} debía pasar del nivel " +
                        $"{nivelAnterior} al {nivelEsperado}, pero terminó en el {nivelFinal}.");
                }

                pokemonEsperados++;
                if (nivelAnterior == NivelMaximoEntrenador)
                {
                    yaEnMaximoEsperados++;
                }
                else if (nivelEsperado != nivelAnterior)
                {
                    aumentadosEsperados++;
                    if (nivelEsperado == NivelMaximoEntrenador)
                        alcanzaronMaximoEsperados++;
                }
            }
        }

        ValidarContadorNivel(nameof(resultado.EntrenadoresProcesados),
            entrenadoresEsperados, resultado.EntrenadoresProcesados);
        ValidarContadorNivel(nameof(resultado.PokemonProcesados),
            pokemonEsperados, resultado.PokemonProcesados);
        ValidarContadorNivel(nameof(resultado.PokemonAumentados),
            aumentadosEsperados, resultado.PokemonAumentados);
        ValidarContadorNivel(nameof(resultado.PokemonQueAlcanzaronNivelMaximo),
            alcanzaronMaximoEsperados, resultado.PokemonQueAlcanzaronNivelMaximo);
        ValidarContadorNivel(nameof(resultado.PokemonYaEnNivelMaximo),
            yaEnMaximoEsperados, resultado.PokemonYaEnNivelMaximo);
    }

    private static ushort CalcularNivelEsperadoParaValidacion(ushort nivelAnterior)
    {
        // Ruta decimal independiente del cálculo entero usado por la transformación.
        decimal nivelExacto = nivelAnterior * 1.17m;
        decimal nivelRedondeado = Math.Round(nivelExacto, 0, MidpointRounding.AwayFromZero);
        return (ushort)Math.Min(nivelRedondeado, NivelMaximoEntrenador);
    }

    private static void ValidarContadorNivel(string nombre, int esperado, int obtenido)
    {
        if (esperado != obtenido)
        {
            throw new InvalidDataException(
                $"El contador {nombre} indica {obtenido}, pero se esperaban {esperado}.");
        }
    }

    private static bool BytesIgualesNivel(byte[] primero, byte[] segundo)
    {
        if (ReferenceEquals(primero, segundo))
            return true;
        return primero is not null && segundo is not null && primero.AsSpan().SequenceEqual(segundo);
    }

    private sealed class ResultadoSubidaNiveles
    {
        public int EntrenadoresProcesados { get; set; }
        public int PokemonProcesados { get; set; }
        public int PokemonAumentados { get; set; }
        public int PokemonQueAlcanzaronNivelMaximo { get; set; }
        public int PokemonYaEnNivelMaximo { get; set; }
    }
}
