import React from 'react';
import logo from './logo.svg';
import './App.css';

function App() {
  return (
    <div className="App">
      <div className="container login-container">
        <div className="row">
          <div className="col"></div>
          <div className="col-md-7">
            <div className="card login-card">
              <div className="card-block">
                <h4 className="card-title login-title">Gestão de Florestas</h4>
                <p className="card-text login-text">Website dedicado à gestão da limpeza e organização de florestas e territórios.</p>
                <form className="login-form">
                  <div className="form-group">
                    <input type="email" className="form-control" id="emailInput" placeholder="Email"></input>
                  </div>
                  <div className="form-group">
                    <input type="password" className="form-control" id="passwordInput" placeholder="Password"></input>
                  </div>
                  <button type="button" className="btn login-btn btn-success btn-sm">Login</button>
                  <button type="button" className="btn login-btn btn-success btn-sm">Registar</button>
                </form>
              </div>
            </div>
          </div>
          <div className="col"></div>
        </div>
      </div>

    </div>
  );
}

export default App;
