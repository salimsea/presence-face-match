import { Layout } from "../components";
import { instance } from "../helpers";
import Swal from "sweetalert2";
import { useForm } from "react-hook-form";

const Login = () => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();

  const btnLogin = (data) => {
    var fd = new FormData();
    fd.append("usernameOrEmail", data.email);
    fd.append("password", data.password);
    instance.post("/auth/login", fd).then((res) => {
      var data = res.data;
      if (data.isSuccess) {
        Swal.fire("Berhasil", `Selamat datang!`, "success").then(() => {
          localStorage.setItem("TOKEN", data.data.token);
          window.location = "/";
        });
      } else {
        Swal.fire("Gagal", data.returnMessage, "error");
      }
    });
  };
  return (
    <Layout>
      <center>
        <div className="w-full max-w-sm p-4 bg-white border border-gray-200 rounded-lg shadow sm:p-6 md:p-8 dark:bg-gray-800 dark:border-gray-700">
          <form
            className="space-y-6"
            onSubmit={handleSubmit(btnLogin)}
            action="#"
          >
            <h5 className="text-xl font-medium text-gray-900 dark:text-white">
              Sign in to our platform
            </h5>
            <div>
              <label
                htmlFor="email"
                className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
              >
                Your email
              </label>
              <input
                type="email"
                name="email"
                id="email"
                className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white"
                placeholder="name@company.com"
                required
                {...register("email", { required: true })}
              />
              {errors.email && (
                <span className="text-sm text-red-500">
                  This field is required
                </span>
              )}
            </div>
            <div>
              <label
                htmlFor="password"
                className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
              >
                Your password
              </label>
              <input
                type="password"
                name="password"
                id="password"
                placeholder="••••••••"
                className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white"
                required
                {...register("password", { required: true })}
              />
              {errors.nama && (
                <span className="text-sm text-red-500">
                  This field is required
                </span>
              )}
            </div>

            <button
              type="submit"
              className="w-full text-white bg-[#9B5DD6] hover:bg-[#7747a4] focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
            >
              Login to your account
            </button>
          </form>
        </div>
      </center>
    </Layout>
  );
};

export default Login;
