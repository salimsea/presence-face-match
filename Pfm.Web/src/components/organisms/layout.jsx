import { Footer, Header } from "../molecules";

const Layout = ({ children }) => {
  return (
    <div>
      <Header />
      <div className="container mx-auto md:px-8 px-4 2xl:px-28">{children}</div>
      <Footer />
    </div>
  );
};

export default Layout;
