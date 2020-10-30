using BenchmarkDotNet.Running;
using MySql.Data.MySqlClient;
using System;
using System.Reflection;

namespace PerformanceTest
{
	public static class Program
	{
		public static void Main(string[] args)
		{
#if DEBUG
			WriteLineColor("Warning: DEBUG configuration; performance may be impacted!", ConsoleColor.Red);
			Console.WriteLine();
#endif

            TestConfig.Instance.Init();
			EnsureDatas();

			BenchmarkSwitcher.FromAssembly(Assembly.GetEntryAssembly()).Run(args, new BenchmarkConfig());
			Console.ReadLine();
		}

		/// <summary>
		/// 预存一些数据
		/// </summary>
		private static void EnsureDatas()
		{
			using (var cnn = new MySqlConnection(TestConfig.Instance.ConnectionString))
			{
				cnn.Open();
				var cmd = cnn.CreateCommand();
				cmd.CommandText = $@"
DROP TABLE
IF
	EXISTS TestDatas;
CREATE TABLE
IF
	NOT EXISTS TestDatas (
		`Id` INT ( 1 ) NOT NULL AUTO_INCREMENT,
		`Text` text NOT NULL,
		`CreationDate` datetime(4) NOT NULL,
		`LastChangeDate` datetime(4) NOT NULL,
		`Counter1` INT ( 1 ) NULL,
		`Counter2` INT ( 1 ) NULL,
		`Counter3` INT ( 1 ) NULL,
		`Counter4` INT ( 1 ) NULL,
		`Counter5` INT ( 1 ) NULL,
		`Counter6` INT ( 1 ) NULL,
		`Counter7` INT ( 1 ) NULL,
		`Counter8` INT ( 1 ) NULL,
		`Counter9` INT ( 1 ) NULL,
		PRIMARY KEY ( `Id` ) 
	);

DROP PROCEDURE
IF
	EXISTS test_while_001;
CREATE PROCEDURE test_while_001 () BEGIN
	DECLARE
		i INT DEFAULT 1;
	WHILE
			i <= {TestConfig.Instance.RowCount} DO
			INSERT INTO TestDatas ( Text, CreationDate, LastChangeDate )
		VALUES
			(
			REPEAT
					( 'x', 2000 ),
					now(),
				now());
			
			SET i = i + 1;
			
		END WHILE;
		
	END;

	CALL test_while_001 ();
	DROP PROCEDURE test_while_001;

    ALTER TABLE TestDatas MODIFY COLUMN `Id` int(1) NOT NULL FIRST;
";
				cmd.Connection = cnn;
				cmd.ExecuteNonQuery();
			}
		}

		private static void WriteLineColor(string message, ConsoleColor color)
		{
			var orig = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.WriteLine(message);
			Console.ForegroundColor = orig;
		}

		private static void WriteColor(string message, ConsoleColor color)
		{
			var orig = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.Write(message);
			Console.ForegroundColor = orig;
		}
	}
}
