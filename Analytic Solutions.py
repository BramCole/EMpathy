import numpy as np
import matplotlib.pyplot as plt

def pointCharge(q, r_s, r_m):
    ### Calculates the EM field of an assembly of point charges ###
    
    #Each pc in pc_ass has 4 dimensions
    #pc[0] == charge
    #pc[1:end] == source position
    #r_m == measurement position from the origin
    k=9e9 #Initialize
    #Define the distance from the source to our measurement
    r = r_m - r_s
    rSqrd = r[:,0]**2 + r[:,1]**2 + r[:,2]**2
    
    #Ignore the points that cause an infinite electromagnetic field
    inf_ind = np.where(rSqrd==0)
    q = np.delete(q,inf_ind)
    rSqrd = np.delete(rSqrd,inf_ind)
    
    r = np.delete(r,inf_ind, axis=0)
    
    #Reshape rSqrd and q for vectorised calculations
    rSqrd = np.reshape(rSqrd,(len(rSqrd),1))
    q = np.reshape(q,(len(q),1))
    
    #Define the unit vector of the EM field
    rHat = r/rSqrd**0.5
    
    #Use Coulomb's Law to analytically calculate the EM field
    E_array = k*q/rSqrd * rHat
    
    return  np.array([np.sum(E_array[:,0]), np.sum(E_array[:,1]), np.sum(E_array[:,2])]) #Add the contribution from each point charge

def lineCharge(L, q_tot, r_s0, v_s0, r_m, N):
    ### Calculates the EM field of a finite line charge ###
    
    #L == length
    #q_tot == total charge carried by the wire
    #r_s0 == point at one end of the line charge
    #v_s0 == unit vector oriented along the line charge (relative to the origin)
    #r_m == radius of measurement (relative to the origin)
    #N == number of sample points
    
    #Ensure that v_s0 is a unit vector
    v_s0 = v_s0/np.dot(v_s0,v_s0)**0.5
    
    dq = np.ones(N) * (q_tot/N)
    range_ = np.linspace(0,L,N)
    r_s = r_s0 + np.array([v_s0[0]*range_, v_s0[1]*range_, v_s0[2]*range_]).T
    
    return pointCharge(dq, r_s, r_m), r_s

def plateCharge(L, q_tot, z, r_m, N):
    ## Measures the electric field produced by a plate orinted in the x-y plane ##
    #L == length of the square plate
    #q_tot == total charge carried by the plate
    #z is the position of the plate
    #r_m == radius of measurement (relative to the origin)
    #N == number of sample points
    
    #Define the differential charge
    dq = q_tot/N**2
    
    #Define the array of source points
    rn = np.linspace(0,L,N)
    x,y=np.meshgrid(rn,rn)
    X = np.reshape(x,(N,N,1))
    Y = np.reshape(y,(N,N,1))
    
    r_sAr = np.append(X,Y,axis=2)
    r_s = np.reshape(r_sAr,(N**2,2)) #Flatten
    
    Z = np.ones((N**2,1)) * z #Make it 3D
    r_s = np.append(r_s,Z,axis=1)
    
    return pointCharge(dq, r_s, r_m)

def capacitor(L, d, V, C, r_m, N):
    #Total charge on the place of a capacitor
    Q = C*V
    #Sum the contributions of each plate
    E = plateCharge(L=L, q_tot=Q, z=d, r_m=r_m, N=N) + plateCharge(L=L, q_tot=-Q, z=0, r_m=r_m, N=N)
    return E

def capField(L, d, V, C, N, n_m):
    
    #Plot the capacitor
    plt.plot(np.linspace(0,L,10),np.ones(10)*d,'+')
    plt.plot(np.linspace(0,L,10),np.zeros(10),'_')
    
    #Define the points to measure the field
    rnX = np.linspace(-L,2*L,n_m)
    rnZ = np.linspace(-d,2*d,n_m)
    range_ = np.array([0,L/8])
    
    #Measure the field at multiple points
    for i in range(n_m):
        for k in range(n_m):
        #Define the measurement point
            r_m = np.array([rnX[i], L/2, rnZ[k]])
            #print(r_m)

            #Measure the electric field at that point
            E_i = capacitor(L, d, V, C, r_m, N)
            magE_i = np.dot(E_i,E_i)**0.5 #Save for colour of vector
            E_i = E_i/magE_i #Normalize for ease of display

            #Create the electric field vectors
            Efield = r_m + np.array([E_i[0]*range_, E_i[1]*range_, E_i[2]*range_]).T
            plt.plot(Efield[:,0], Efield[:,2], 'b', linewidth=magE_i/10)
            plt.scatter(r_m[0],r_m[2], c='b', s=1)
    
    plt.xlabel('x')
    plt.ylabel('z')
    plt.show()