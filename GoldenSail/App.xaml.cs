using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GoldenSail.Entities;

namespace GoldenSail
{
    /// <summary>
    /// Запускает приложение и хранит общий контекст базы данных.
    /// </summary>
    public partial class App : Application
    {
        // Глобальный контекст для работы с базой данных
        public static GoldenSailEntities Context = new GoldenSailEntities();
    }
}
