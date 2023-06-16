const Footer = () => {
  return (
    <footer className="bg-gray-800 shadow dark:bg-gray-800 absolute bottom-0 w-full">
      <div className="w-full mx-auto max-w-screen-xl p-4 md:flex md:items-center md:justify-between">
        <span className="text-sm text-gray-500 sm:text-center dark:text-gray-400">
          © 2023{" "}
          <a href="/" className="hover:underline">
            SIPM (Project Minor)
          </a>
          . All Rights Reserved.
        </span>
      </div>
    </footer>
  );
};

export default Footer;
