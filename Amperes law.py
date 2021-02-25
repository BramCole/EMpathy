import numpy as np
from numpy import sin
from numpy import cos
import matplotlib.pyplot as plt
from scipy.constants import *
from tqdm import tqdm

def lineCurrent(theta, phi, L, I, N, rm):
  ### Measures the magnetic field induced by a line of current ###
  ## The line of current is assumed to start at the origin. It can then be moved ##

  # theta ==  angle between the +x-axis and and the line of current
  # phi   ==  angle between the +y-axis and the line of current
  # L     ==  length of the current carrying wire
  # I     ==  total current of the wire (might be variable in the future) 
  # N     ==  Number of sample points for the integral
  # rm    ==  the point at which we measure the magnetic field

  #Define the source points along the line of current
  range_ = np.linspace(0,L,N)
  rs = np.array([sin(phi)*cos(theta)*range_, sin(phi)*sin(theta)*range_, cos(phi)*range_]).T

  #Define the displacement between the wire and the point of measurement
  r = rm - rs
  rSqrd = r[:,0]**2 + r[:,1]**2 + r[:,2]**2

  #Ignore the points that cause an infinite electromagnetic field
  inf_ind = np.where(rSqrd==0)
  r = np.delete(r,inf_ind, axis=0)
  rSqrd = np.delete(rSqrd,inf_ind)

  #Reshape rSqrd for vectorised calculations
  rSqrd = np.reshape(rSqrd,(len(rSqrd),1))

  r_hat = r/rSqrd**0.5
  N_new = len(r_hat)

  #Define the differential quantities to be used in the integral
  dl = np.array([sin(phi)*cos(theta), sin(phi)*sin(theta), cos(phi)])*L/N_new
  
  #Find the magnetic field induced by each segment of the wire
  B_ar = (mu_0*I/(4*pi)) * (np.cross(dl, r_hat) / rSqrd)
  B_ar = (B_ar[:-1] + B_ar[1:])/2 #Trapezoidal rule
  return np.sum(B_ar,axis=0) #integrate

def solenoid(l, I, N_turns, r, N, rm):
  ### Measures the magnetic field induced by a solenoid ###
  ## The solenoid is assumed to start at the origin. It can then be moved in Unity ##

  # l       ==  length of the solenoid in the axial direction
  # I       ==  total current of the wire (might be variable in the future) (Positive current means I points up at the origin end of the solenoid)
  # N_turns ==  number of turns in the solenoid
  # r       ==  radius of the solenoid turns
  # N       ==  Number of sample points for the integrals
  # rm      ==  the point at which we measure the magnetic field

  #Define the source points along the solenoid
  z = np.linspace(0,l,N) # z is the parametric variable
  w = 2*pi*N_turns/(l - l/(2*N_turns)) # Angular frequency in the x-y plane
  rs = np.array([-r*cos(w*z), -r*sin(w*z), z]).T

  #Define the displacement between the wire and the point of measurement
  r = rm - rs
  rSqrd = r[:,0]**2 + r[:,1]**2 + r[:,2]**2

  #Reshape rSqrd for vectorised calculations
  rSqrd = np.reshape(rSqrd,(len(rSqrd),1))
  r_hat = r/rSqrd**0.5

  #Determine the total length of the solenoid wire (using arc length) to find the differential length of dl
  L_ar = np.sqrt(w**2 + z**2)
  dL = l/N
  L_ar = (L_ar[:-1] + L_ar[1:])*dL/2 #Trapezoidal rule
  L = np.sum(L_ar, axis=0)
  #Define the dl vector for the cross product calculation
  dl = (L/N) * np.array([-sin(w*z), cos(w*z), -z]) * np.sign(I) # np.sign(I) allows for dl to change direction depending on the current
  
  #Find the magnetic field induced by each segment of the wire
  B_ar = (mu_0*I/(4*pi)) * (np.cross(dl.T, r_hat) / rSqrd)
  B_ar = (B_ar[:-1] + B_ar[1:])/2 #Trapezoidal rule

  return np.sum(B_ar,axis=0) #integrate