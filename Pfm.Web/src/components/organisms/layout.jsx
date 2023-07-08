import { Footer, Header } from "../molecules";

// eslint-disable-next-line react/prop-types
const Layout = ({ children }) => {
  return (
    <div className="bg-[#F6F7FC]">
      <Header />
      <div className="container mx-auto md:px-8 px-4 2xl:px-28 min-h-[79vh]">
        {children}
      </div>
      <Footer />
    </div>
  );
};

export default Layout;
