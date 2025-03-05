using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace _4994880_GRUDINDIVUAL
{
	public partial class Form1 : Form
	{

		public Form1()
		{
			InitializeComponent();
		}

		//Variable principal para empezar la conexion con la base de datos
		System.Data.OleDb.OleDbConnection conBD = new System.Data.OleDb.OleDbConnection();
		String sAction = "";

		public void OpenConnection()
		{
			//Abrir la conexion con la base de datos y mostrar si hay fallo en ello 
			conBD.ConnectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Users\MINEDUCYT\Desktop\PRACTICA 3\WindowsFomandAccess\Empresa.accdb";

			try
			{
				conBD.Open();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error de conexion" + ex);
			}
		}

		public void CloseConnection()
		{
			//Se chequea si la conexion esta habilitada y luego la cierra
			if (conBD.State == ConnectionState.Open)
			{
				conBD.Close();
			}
		}

		private void desactivarBotones()
		{
			btnConsulta.Enabled = false;
			btnIngreso.Enabled = false;
			btnEliminacion.Enabled = false;
			btnModificacion.Enabled = false;

			txtDiauno.Enabled = false;
			txtDiados.Enabled = false;
			txtDiatres.Enabled = false;
			txtDiacuatro.Enabled = false;
			txtDiacinco.Enabled = false;
			txtDiaseis.Enabled = false;

			btnAceptar.Enabled = true;
			btnCancelar.Enabled = true;
		}

		private void activarBotones()
		{
			btnConsulta.Enabled = true;
			btnIngreso.Enabled = true;
			btnEliminacion.Enabled = true;
			btnModificacion.Enabled = true;
			btnAceptar.Enabled = false;
			btnCancelar.Enabled = false;

			// Se cancela la accion a realizar
			sAction = "";
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			OpenConnection();
		}


		private void btnCalcular_Click(object sender, EventArgs e)
		{

			//Calcular el total de horas laboradas
			int HorasT = (int.Parse(txtDiauno.Text) + int.Parse(txtDiados.Text) + int.Parse(txtDiatres.Text) + int.Parse(txtDiacuatro.Text)
				+ int.Parse(txtDiacinco.Text) + int.Parse(txtDiaseis.Text));

			//Calcular el salario
			double Cantidadsalario = 1.50;
			double Salario = Cantidadsalario * HorasT;

			//Mostrando resultados

			txtTotal.Text = HorasT.ToString();
			txtSalario.Text = Salario.ToString();
		}

		private void btnRegistro_Click(object sender, EventArgs e)
		{
			txtEmpleado.Text = "";
			txtDiauno.Text = "";
			txtDiados.Text = "";
			txtDiatres.Text = "";
			txtDiacuatro.Text = "";
			txtDiacinco.Text = "";
			txtDiaseis.Text = "";
			txtTotal.Text = "";
			txtSalario.Text = "";
		}

		private void btnSalir_Click(object sender, EventArgs e)
		{
			//S edebe cerrar la conexion
			CloseConnection();

			//Se cierra el programa
			this.Close();
		}

		private void btnConsulta_Click(object sender, EventArgs e)
		{
			//Se indica la accion a realizar
			sAction = "Consulta";
			//Desactivamos los botones
			desactivarBotones();
		}

		private void btnIngreso_Click(object sender, EventArgs e)
		{
			//Se indica la accion a realizar
			sAction = "Ingreso";
			//Desactivamos los botones
			desactivarBotones();
		}

		private void btnEliminacion_Click(object sender, EventArgs e)
		{
			//Accion que se realizara
			sAction = "Eliminacion";
			//Desactivar botones
			desactivarBotones();
		}

		private void btnModificacion_Click(object sender, EventArgs e)
		{
			//Accion que se realizara
			sAction = "Modificacion";
			//Fuera botones
			desactivarBotones();
		}

		private void btnCancelar_Click(object sender, EventArgs e)
		{
			activarBotones();
		}

		private void btnAceptar_Click(object sender, EventArgs e)
		{

			//----------------------------------CONSULTA-------------------------------
			if (sAction == "Consulta")
			{
				try
				{
					//Se realiza la consulta
					String sConsulta = "SELECT *  "
						+ "FROM Empleados  "
						+ "WHERE Empleado = '" + txtEmpleado.Text + "'";

					OleDbCommand oleComando = new OleDbCommand(sConsulta, conBD);
					OleDbDataReader oleReader = oleComando.ExecuteReader();

					//Se lee el resultado para ver si existe alguno
					if (oleReader.Read())
					{
						//SE muestra los resultados
						txtTotal.Text = Convert.ToString(oleReader["Horas Trabajadas"]);
						txtSalario.Text = Convert.ToString(oleReader["Salario"]);
					}
					else
					{
						//Si no existe dar un mensaje
						MessageBox.Show("El empleado no esta registrado");
					}
				}
				catch (Exception ex)
				{
					//Mensaje de error
					MessageBox.Show("Valor ingresado invaliudo" + ex);

				}
			}

			//----------------------------------CODIGO DE INGRESO-------------------------

			else
			{
				if (sAction == "Ingreso")
				{
					try
					{
						//Se inserta el codigo 
						String sConsulta = "INSERT INTO Empleados "
							+ " VALUES ('" + txtEmpleado.Text + "'," + txtTotal.Text + "," + txtSalario.Text + ")";

						//Se escribe el codigo para hacer efectiva la consulta y quese ejute
						OleDbCommand oleComando = new OleDbCommand();
						oleComando.CommandType = CommandType.Text;
						oleComando.CommandText = sConsulta;
						oleComando.Connection = conBD;

						// Se ejecuta
						oleComando.ExecuteNonQuery();

						//Limpiamos los textboxs

						txtEmpleado.Text = "";
						txtTotal.Text = "";
						txtSalario.Text = "";


					}
					catch (Exception ex)
					{
						//MOstra mensaje de error si hay pronlemas al ingresar
						MessageBox.Show("No es posible ingresar datos en la base" + ex);
					}
				}
				else
				{
					//----------------------------------CODIGO DE ELIMINACION-----------------------------------
					if (sAction == "Eliminacion")
					{
						try
						{
							//Se escribe la consulta
							String sConsulta = "DELETE FROM Empleados "
												 + " WHERE Empleado = '" + txtEmpleado.Text + "'";

							OleDbCommand oleComando = new OleDbCommand();
							oleComando.CommandType = CommandType.Text;
							oleComando.CommandText = sConsulta;
							oleComando.Connection = conBD;

							//Se ejcuta la conulta
							oleComando.ExecuteNonQuery();

							//Limpiamos los textBox

							txtEmpleado.Text = "";
							txtTotal.Text = "";
							txtSalario.Text = "";
						}
						catch (Exception ex)
						{
							//MOstrar mensaje si hay fallo
							MessageBox.Show("No se pudo eliminar el registro" + ex);
						}
					}
					else
					{
							try
							{
								// Se escribe la sintaxis
								String sConsulta = "UPDATE Empleados "
									+ " SET Salario = " + txtSalario.Text + " , "
									+ " [Horas Trabajadas] = " + txtTotal.Text + " "
									+ " WHERE Empleado = '" + txtEmpleado.Text+"' ";

							OleDbCommand oleComando = new OleDbCommand();
								oleComando.CommandType = CommandType.Text;
								oleComando.CommandText = sConsulta;
								oleComando.Connection = conBD;

								//Se ejcuta la conulta
								oleComando.ExecuteNonQuery();

								//Limpiamos los textBox

								txtEmpleado.Text = "";
								txtTotal.Text = "";
								txtSalario.Text = "";
							}
							catch (Exception ex)
							{
								MessageBox.Show("No se pudo modificar el registro" + ex);
							}
						
					}
				}
			}
			//Se termina la accion
			sAction = "";

			activarBotones();
		}
	}
}


