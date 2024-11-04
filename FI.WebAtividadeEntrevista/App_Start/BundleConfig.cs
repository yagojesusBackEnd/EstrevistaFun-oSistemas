using System.Web.Optimization;

namespace FI.WebAtividadeEntrevista
{
	public class BundleConfig
    {
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			// Bundle para jQuery e jQuery Mask Plugin
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-3.4.1.min.js",
						"~/Scripts/jquery.mask.min.js")); // Inclui o jQuery Mask Plugin

			bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
						"~/Scripts/jquery-ui-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Modernizr
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			// Bootstrap
			bundles.Add(new Bundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js"));

			// jTable
			bundles.Add(new ScriptBundle("~/bundles/jtable").Include(
					  "~/Scripts/jtable/jquery.jtable.min.js",
					  "~/Scripts/jtable/localization/jquery.jtable.pt-BR.js"));
			
			// Estilos CSS
			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));

			// Estilos para jTable
			bundles.Add(new StyleBundle("~/Content/jtable").Include(
					  "~/Scripts/jtable/themes/metro/darkgray/jtable.css"));

			// Scripts específicos de Clientes
			bundles.Add(new ScriptBundle("~/bundles/clientes").Include(
					  "~/Scripts/Clientes/FI.Clientes.js"));

			bundles.Add(new ScriptBundle("~/bundles/listClientes").Include(
					  "~/Scripts/Clientes/FI.ListClientes.js"));

			bundles.Add(new ScriptBundle("~/bundles/altClientes").Include(
					  "~/Scripts/Clientes/FI.AltClientes.js"));

			//Scripts especificos de Beneficiarios
			bundles.Add(new ScriptBundle("~/bundles/beneficiarios").Include(
					  "~/Scripts/Beneficiarios/FI.Beneficiarios.js"));
		}
	}
}
